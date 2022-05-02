using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Domain.Interfaces.Infra
{
    public interface INotify
    {
        bool IsValid { get; }
        void CleanErrors();
        void Validate(Func<bool> funcValidate, string error, bool continueError = true);
        string GetConcatErrors(char concatChar = '|');
    }
}
