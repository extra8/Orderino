using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Orderino.Utils;
using System.Threading;
using System.Threading.Tasks;

namespace Orderino.Cards
{
    public class CardBase
    {

        public async Task<IMessageActivity> SendLoadingCard(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var cardAttachment = GetLoadingCard();
            var activity = MessageFactory.Attachment(cardAttachment);
            activity.Id = turnContext.Activity.Id;
            await turnContext.SendActivityAsync(activity, cancellationToken);

            return activity;
        }
        private Attachment GetLoadingCard()
        {
            var card = new ThumbnailCard
            {
                Title = "Loading...",
                Subtitle = "Please wait."
            };

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = Constants.AdaptiveCardContentType,
                Content = card,
            };

            return adaptiveCardAttachment;
        }
    }
}
