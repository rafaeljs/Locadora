using Locadora.Domain.Interfaces.Infra;

namespace Locadora.Domain.Infra
{
    public abstract class Notify : INotify
    {
        public bool IsValid { get; private set; }
        private List<string> _messagesError;

        protected Notify()
        {
            IsValid = true;
            _messagesError = new List<string>();
        }

        public void AddError(string error)
        {
            IsValid = false;
            _messagesError.Add(error);
        }

        public void CleanErrors()
        {
            IsValid = true;
            _messagesError = new List<string>();
        }

        public void Validate(Func<bool> funcValidate, string error, bool continueError = true)
        {
            if (!continueError && !IsValid) return;

            if (funcValidate())
            {
                AddError(error);
            }
        }

        public string GetConcatErrors(char concatChar = '|') => string.Join(concatChar, _messagesError);
    }
}
