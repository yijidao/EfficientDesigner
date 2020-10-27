using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EfficientDesigner_Control.Commands;
using EfficientDesigner_Service.Models;
using EfficientDesigner_Service.Services;
using Prism.Mvvm;
using Prism.Regions;

namespace EfficientDesigner_Shell.ViewModels
{
    public class LayoutListViewModel : BindableBase
    {
        private readonly ILayoutService _layoutService;

        public LayoutListViewModel( ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        private ObservableCollection<Layout> _layouts;
        public ObservableCollection<Layout> Layouts
        {
            get { return _layouts; }
            set { SetProperty(ref _layouts, value); }
        }

        public ICommand LoadedCommand => new DelegateCommand(async () =>
        {
            Layouts = new ObservableCollection<Layout>(await _layoutService.GetLayouts());
        });

    }
}
