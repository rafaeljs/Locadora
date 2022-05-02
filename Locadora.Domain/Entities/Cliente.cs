using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Domain.Entities
{
    public class Cliente : EntityBase
    {
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public DateTime DataNascimento { get; private set; }

        public virtual ICollection<Locacao> Locacoes { get; private set; }

        protected Cliente() { }
        public Cliente(string nome, string cpf, DateTime dataNascimento)
        {
            Nome = nome;
            CPF = cpf;
            DataNascimento = dataNascimento;
        }

        public void AlterarNome(string nome) => Nome = nome;
        public void AlterarCPF(string  cpf) => CPF = cpf;
        public void AlterarDataNascimento(DateTime dataNascimento) => DataNascimento = dataNascimento;
    }
}
