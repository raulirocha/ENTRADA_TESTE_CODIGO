// Em: FluxoSistema.Entrada/ViewModels/ItemEntradaViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using FluxoSistema.Core.Models; // Precisa desta referência

namespace FluxoSistema.Entrada.ViewModels
{
    public partial class ItemEntradaViewModel : ObservableObject
    {
        // Guarda a referência ao item original que veio do XML da nota
        public DfeItemSincronizado ItemOriginal { get; }

        // Esta propriedade irá guardar o produto do SEU sistema depois que for vinculado
        [ObservableProperty]
        private Produto _produtoVinculado;

        // Construtor que recebe o item da nota
        public ItemEntradaViewModel(DfeItemSincronizado item)
        {
            ItemOriginal = item;
        }
    }
}