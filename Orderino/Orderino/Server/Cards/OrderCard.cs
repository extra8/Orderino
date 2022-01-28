using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Orderino.Server.Cards.Utils;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orderino.Server.Cards
{
    public class OrderCard : CardBase
    {
        public async Task SendOrderCard(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            IMessageActivity activity = await SendLoadingCard(turnContext, cancellationToken);

            string activityId = turnContext.Activity.Id;
            Attachment cardAttachment = GetOrderCard(turnContext, cancellationToken, activityId);
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
                    })
                }
            };

            return heroCard.ToAttachment();
        }
    }
}
