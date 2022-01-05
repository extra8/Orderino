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
        private readonly SessionCache sessionCache;

        public OrderinoBot(IConfiguration configuration, ILogger<OrderinoBot> logger, Repository<User> userRepository, SessionCache sessionCache)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.userRepository = userRepository;
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
                string command = turnContext.Activity.Text.Trim().Split(' ').Last().ToLower();
                switch (command)
                {
                    case "help":
                        logger.LogInformation("Sending a loading card");
                        HelpCard helpCard = new HelpCard();
                        await helpCard.SendHelpCard(turnContext, cancellationToken);
                        break;
                    default: break;
                }

                await StoreUsers(turnContext, cancellationToken);
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

            if (action.Type == "help")
            {
                return GetHelpTaskModule(turnContext, cancellationToken);
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
                Width = 1280,
                Height = 720,
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
