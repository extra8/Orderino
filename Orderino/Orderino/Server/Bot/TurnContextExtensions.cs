using Microsoft.Bot.Schema;

namespace Orderino.Server.Bot
{
    public static class TurnContextExtensions
    {
        public static string GetActionType(this IInvokeActivity activity)
        {
            try
            {
                var action = (dynamic) activity.Value;

                dynamic data = action.data;
                if (data == null)
                    return "unknown";

                data = data.type;
                if (data == null)
                    return "unknown";

                return data.Value;
            }
            catch
            {
                return "unknown";
            }
        }
    }
}
