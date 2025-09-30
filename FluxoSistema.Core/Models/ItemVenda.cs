using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("pdv_itens_venda")]
    public partial class ItemVenda : ObservableObject
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("idvenda")]
        public int VendaId { get; set; }

        [Column("produtoid")]
        public int ProdutoId { get; set; }

        [Column("codigobarras")]
        public string? CodigoBarras { get; set; }

        private string? _descricao;
        [Column("descricaoproduto")]
        public string? Descricao
        {
            get => _descricao;
            set => SetProperty(ref _descricao, value);
        }

        private decimal _quantidade;
        [Column("quantidade")]
        public decimal Quantidade
        {
            get => _quantidade;
            set
            {
                if (SetProperty(ref _quantidade, value))
                {
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }

        private decimal _precoUnitario;
        [Column("precounitario")]
        public decimal PrecoUnitario
        {
            get => _precoUnitario;
            set
            {
                if (SetProperty(ref _precoUnitario, value))
                {
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }

        private string _statusItem = "Ativo";
        [Column("statusitem")]
        public string StatusItem
        {
            get => _statusItem;
            set => SetProperty(ref _statusItem, value);
        }

        [NotMapped]
        public int ItemNumero { get; set; }

        [NotMapped]
        public string? CodigoProduto { get; set; }

        [NotMapped]
        public decimal Subtotal => Quantidade * PrecoUnitario;
    }
}