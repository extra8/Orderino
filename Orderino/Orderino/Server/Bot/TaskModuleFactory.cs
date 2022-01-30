using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using Orderino.Shared.Models.Enums;

namespace Orderino.Server.Bot
{
    public static class TaskModuleFactory
    {
        public static InvokeResponse GetTaskModule(TaskModuleType type, ITurnContext<IInvokeActivity> turnContext, IConfiguration configuration)
        {
            switch (type)
            {
                case TaskModuleType.Help:
                    return GetHelpTaskModule(turnContext, configuration);

                case TaskModuleType.Order:
                    return GetOrderTaskModule(turnContext, configuration);

                case TaskModuleType.History:
                    return GetHistoryTaskModule(turnContext, configuration);

                default:
                    return GetHelpTaskModule(turnContext, configuration);
            }
        }

        private static InvokeResponse GetHelpTaskModule(ITurnContext<IInvokeActivity> turnContext, IConfiguration configuration)
        {
            var taskModule = new TaskModuleTaskInfo
            {
                Url = configuration["BaseUrl"],
                FallbackUrl = configuration["BaseUrl"],
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

        private static InvokeResponse GetOrderTaskModule(ITurnContext<IInvokeActivity> turnContext, IConfiguration configuration)
        { 
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

        private static InvokeResponse GetHistoryTaskModule(ITurnContext<IInvokeActivity> turnContext, IConfiguration configuration)
        {
            var taskModule = new TaskModuleTaskInfo
            {
                Url = configuration["BaseUrl"] + $"/history/{turnContext.Activity.Conversation.Id}/{turnContext.Activity.From.AadObjectId}",
                FallbackUrl = configuration["BaseUrl"] + $"/history/{turnContext.Activity.Conversation.Id}/{turnContext.Activity.From.AadObjectId}",
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
