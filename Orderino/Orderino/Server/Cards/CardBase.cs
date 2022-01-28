using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Orderino.Server.Cards
{
    public class CardBase
    {
        private const string adaptiveCardContentType = "application/vnd.microsoft.card.thumbnail";

        public async Task<IMessageActivity> SendLoadingCard(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            Attachment cardAttachment = GetLoadingCard();
            IMessageActivity activity = MessageFactory.Attachment(cardAttachment);
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
                ContentType = adaptiveCardContentType,
                Content = card,
            };

            return adaptiveCardAttachment;
        }
    }
}
