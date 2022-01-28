using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Orderino.Server.Cards.Utils;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orderino.Server.Cards
{
    public class HelpCard : CardBase
    {
        public async Task SendHelpCard(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            IMessageActivity activity = await SendLoadingCard(turnContext, cancellationToken);

            string activityId = turnContext.Activity.Id;
            Attachment cardAttachment = GetHelpCard(turnContext, cancellationToken, activityId);
            activity.Attachments.Clear();
            activity.Attachments.Add(cardAttachment);

            await turnContext.UpdateActivityAsync(activity, cancellationToken);
        }

        private Attachment GetHelpCard(ITurnContext turnContext, CancellationToken cancellationToken, string activityId)
        {
            var heroCard = new HeroCard
            {
                Title = "Orderino",
                Subtitle = "Hi! Click the button below to see what I can help you with!",
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
