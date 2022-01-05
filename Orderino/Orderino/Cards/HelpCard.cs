using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Orderino.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orderino.Cards
{
    public class HelpCard : CardBase
    {
        public async Task SendHelpCard(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            IMessageActivity activity = await SendLoadingCard(turnContext, cancellationToken);

            var activityId = turnContext.Activity.Id;
            var cardAttachment = GetHelpCard(turnContext, cancellationToken, activityId);
            activity.Attachments.Clear();
            activity.Attachments.Add(cardAttachment);

            await turnContext.UpdateActivityAsync(activity, cancellationToken);
        }

        private Attachment GetHelpCard(ITurnContext turnContext, CancellationToken cancellationToken, string activityId)
        {
            var heroCard = new HeroCard
            {
                Title = "Orderino Help",
                Subtitle = "How can I help you?",
                Buttons = new List<CardAction>
                {
                    new CardAction(type: "invoke", title: "Help", image: null, text:"", displayText: "",
                    new CardValue<ActionButton>
                    {
                        Data = new ActionButton { ActivityId = activityId, Type = "help" }
                    })
                }
            };

            return heroCard.ToAttachment();
        }
    }
}
