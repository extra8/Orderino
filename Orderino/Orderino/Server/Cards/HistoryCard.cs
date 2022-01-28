using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Orderino.Server.Cards.Utils;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orderino.Server.Cards
{
    public class HistoryCard : CardBase
    {
        public async Task SendHistoryCard(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            IMessageActivity activity = await SendLoadingCard(turnContext, cancellationToken);

            string activityId = turnContext.Activity.Id;
            Attachment cardAttachment = GetHistoryCard(turnContext, cancellationToken, activityId);
            activity.Attachments.Clear();
            activity.Attachments.Add(cardAttachment);

            await turnContext.UpdateActivityAsync(activity, cancellationToken);
        }

        private Attachment GetHistoryCard(ITurnContext turnContext, CancellationToken cancellationToken, string activityId)
        {
            var heroCard = new HeroCard
            {
                Title = "Orderino",
                Subtitle = "Click on the button below to view your past orders.",
                Buttons = new List<CardAction>
                {
                    new CardAction(type: "invoke", title: "History", image: null, text:"", displayText: "",
                    new CardValue<ActionButton>
                    {
                        Data = new ActionButton { ActivityId = activityId, Type = "history" }
                    })
                }
            };

            return heroCard.ToAttachment();
        }
    }
}
