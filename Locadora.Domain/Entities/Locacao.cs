using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Domain.Entities
{
    public class Locacao : EntityBase
    {
        public int ClientId { get; private set; }
        public int FilmeId { get; private set; }
        public DateTime DataLocacao { get; private set; }
        public DateTime? DataDevolucao { get; private set; }

        public virtual Cliente Cliente { get; private set; }
        public virtual Filme Filme { get; private set; }

        protected Locacao() { }

        public Locacao(int clienteId, int filmeId, DateTime dataLocacao)
        {
            ClientId = clienteId;
            FilmeId = filmeId;
            DataLocacao = dataLocacao;
        }

        public void AlterarDataDevolucao(DateTime dataDevolucao) => DataDevolucao = dataDevolucao;
    }
}
