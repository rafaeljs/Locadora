using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Domain.Entities
{
    public class Filme : EntityBase
    {
        public string Titulo { get; private set; }
        public int ClassificacaoIndicativa { get; private set; }
        public bool Lancamento { get; private set; }

        public virtual ICollection<Locacao> Locacoes { get; private set; }

        protected Filme() { }
        public Filme(string titulo, int classificacaoIndicativa, bool lancamento)
        {
            Titulo = titulo;
            ClassificacaoIndicativa = classificacaoIndicativa;
            Lancamento = lancamento;
        }

        public Filme(int id, string titulo, int classificacaoIndicativa, bool lancamento)
        {
            Id = id;
            Titulo = titulo;
            ClassificacaoIndicativa = classificacaoIndicativa;
            Lancamento = lancamento;
        }

        public void AlterarTitulo(string titulo) => Titulo = titulo;
        public void AlterarClassificacao(int classificacao) => ClassificacaoIndicativa = classificacao;
        public void AlterarLancamento(bool lancamento) => Lancamento = lancamento;
    }
}
