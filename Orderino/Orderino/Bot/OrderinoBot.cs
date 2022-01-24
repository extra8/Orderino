using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orderino.Cards;
using Orderino.Models;
using Orderino.Repository;
using Orderino.Repository.Models;
using Orderino.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orderino.Bots
{
    public class OrderinoBot : ActivityHandler
    {
        private readonly ILogger<OrderinoBot> logger;
        private readonly IConfiguration configuration;
        private readonly Repository<User> userRepository;
        private readonly Repository<Order> orderRepository;
        private readonly SessionCache sessionCache;

        public OrderinoBot(IConfiguration configuration, ILogger<OrderinoBot> logger, Repository<User> userRepository, Repository<Order> orderRepository, SessionCache sessionCache)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.userRepository = userRepository;
            this.orderRepository = orderRepository;
            this.sessionCache = sessionCache;
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";

            await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                await StoreUsers(turnContext, cancellationToken);                

                string command = turnContext.Activity.Text.Trim().Split(' ').Last().ToLower();
                switch (command)
                {
                    case "help":
                        logger.LogInformation("Sending a loading card");
                        var helpCard = new HelpCard();
                        await helpCard.SendHelpCard(turnContext, cancellationToken);
                        break;

                    case "order":
                        var order = await orderRepository.QueryItemAsync(turnContext.Activity.Conversation.Id);
                        if (order.Initiator == null)
                        {
                            var user = await userRepository.QueryItemAsync(turnContext.Activity.From.AadObjectId);
                            order.Initiator = user;
                            order.Initiator.Favorites = null;

                            await orderRepository.Update(order);
                        }
                        logger.LogInformation("Sending an order card");
                        var orderCard = new OrderCard();
                        await orderCard.SendOrderCard(turnContext, cancellationToken);
                        break;

                    default: break;
                }                                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error has occured!", turnContext);
                throw;
            }
        }

        protected override async Task<InvokeResponse> OnInvokeActivityAsync(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                switch (turnContext.Activity.GetActionType())
                {
                    default:
                        return await ResponseToButtonClick(turnContext, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error has occured!", turnContext);
                throw;
            }
        }

        private async Task StoreUsers(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var members = new List<TeamsChannelAccount>();
            string continuationToken = null;

            do
            {
                var currentPage = await TeamsInfo.GetPagedMembersAsync(turnContext, 100, continuationToken, cancellationToken);
                continuationToken = currentPage.ContinuationToken;
                members.AddRange(currentPage.Members);
            }
            while (continuationToken != null);

            List<User> users = members.Select(x => new User
            {
                Id = x.AadObjectId,
                FirstName = x.GivenName,
                LastName = x.Surname,
                Username = x.Email
            }).ToList();

            await userRepository.BulkAddAsync(users);
        }

        private async Task<InvokeResponse> ResponseToButtonClick(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            var data = ((dynamic)turnContext.Activity.Value).data.data.ToString();
            ActionButton action = JsonConvert.DeserializeObject<ActionButton>(data);

            switch (action.Type)
            {
                case "help":
                    return GetHelpTaskModule(turnContext, cancellationToken);

                case "order":
                    return await GetOrderTaskModule(turnContext, cancellationToken);
            }

            return await base.OnInvokeActivityAsync(turnContext, cancellationToken);
        }

        private InvokeResponse GetHelpTaskModule(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            var taskModule = new TaskModuleTaskInfo
            {
                Url = configuration["BaseUrl"] + "/counter",
                FallbackUrl = configuration["BaseUrl"] + "/counter",
                Title = "Orderino",
                Width = 1600,
                Height = 900,
                CompletionBotId = configuration["MicrosoftAppId"]
            };

            var tmContinueResponse = new TaskModuleContinueResponse(taskModule);
            var tmResponse = new TaskModuleResponse(tmContinueResponse);
            var invokeResponse = new InvokeResponse
            {
                Body = tmResponse,
                Status = 200
            };

            return invokeResponse;
        }

        private async Task<InvokeResponse> GetOrderTaskModule(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            var order = await orderRepository.QueryItemAsync(turnContext.Activity.Conversation.Id);
            if (order.Initiator == null)
            {
                var user = await userRepository.QueryItemAsync(turnContext.Activity.From.AadObjectId);
                order.Initiator = user;
                order.Initiator.Favorites = null;

                await orderRepository.Update(order);
            }

            var taskModule = new TaskModuleTaskInfo
            {
                Url = configuration["BaseUrl"] + $"/restaurant-browser/{turnContext.Activity.Conversation.Id}/{turnContext.Activity.From.AadObjectId}",
                FallbackUrl = configuration["BaseUrl"] + $"/restaurant-browser/{turnContext.Activity.Conversation.Id}/{turnContext.Activity.From.AadObjectId}",
                Title = "Orderino",
                Width = 1600,
                Height = 900,
                CompletionBotId = configuration["MicrosoftAppId"]
            };

            var tmContinueResponse = new TaskModuleContinueResponse(taskModule);
            var tmResponse = new TaskModuleResponse(tmContinueResponse);
            var invokeResponse = new InvokeResponse
            {
                Body = tmResponse,
                Status = 200
            };

            return invokeResponse;
        }
    }
}
