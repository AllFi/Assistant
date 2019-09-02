
namespace AssistantCore
{
    public class AssistantResponse
    {
        public string Text { get; set; }

        public AssistantResponse( string text )
        {
            Text = text;
        }
    }
}
