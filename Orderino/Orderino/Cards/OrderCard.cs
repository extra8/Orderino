using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Orderino.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orderino.Cards
{
    public class OrderCard : CardBase
    {
        public async Task SendOrderCard(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            IMessageActivity activity = await SendLoadingCard(turnContext, cancellationToken);

            var activityId = turnContext.Activity.Id;
            var cardAttachment = GetOrderCard(turnContext, cancellationToken, activityId);
            activity.Attachments.Clear();
            activity.Attachments.Add(cardAttachment);

            await turnContext.UpdateActivityAsync(activity, cancellationToken);
        }

        private Attachment GetOrderCard(ITurnContext turnContext, CancellationToken cancellationToken, string activityId)
        {
            var heroCard = new HeroCard
            {
                Title = "Orderino",
                Subtitle = "You have started an order. Press \"Order\" to begin selecting your meal or if you are done selecting, or not hungry, on \"Mark as done\".",
                Buttons = new List<CardAction>
                {
                    new CardAction(type: "invoke", title: "Order", image: null, text:"", displayText: "",
                    new CardValue<ActionButton>
                    {
                        Data = new ActionButton { ActivityId = activityId, Type = "order" }
                    }),

                    new CardAction(type: "invoke", title: "Mark as done", image: null, text:"", displayText: "",
                    new CardValue<ActionButton>
                    {
                        Data = new ActionButton { ActivityId = activityId, Type = "markAsDone" }
                    })
                }
            };

            return heroCard.ToAttachment();
        }
    }
}
