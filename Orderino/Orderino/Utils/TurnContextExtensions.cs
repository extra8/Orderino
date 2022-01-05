using Microsoft.Bot.Schema;

namespace Orderino.Utils
{
    public static class TurnContextExtensions
    {
        public static string GetActionType(this IInvokeActivity activity)
        {
            try
            {
                var action = (dynamic)activity.Value;

                var data = action.data;
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
