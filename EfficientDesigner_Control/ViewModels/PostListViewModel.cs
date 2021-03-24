using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Control.Controls;
using EfficientDesigner_Service;
using Prism.Mvvm;

namespace EfficientDesigner_Control.ViewModels
{
    public class PostListViewModel : BindableBase
    {
        private ObservableCollection<PostItenViewModel> _postItems;
        public ObservableCollection<PostItenViewModel> PostItems
        {
            get => _postItems;
            set => SetProperty(ref _postItems, value);
        }

        public PostListViewModel()
        {
            //ServiceFactory.GetLayoutService().GetLayouts();
            //if(DesignerProperties.isin)

            PostItems = new ObservableCollection<PostItenViewModel>
            {
                new PostItenViewModel("标题1"),
                new PostItenViewModel("标题2"),
                new PostItenViewModel("标题3"),
            };

        }
    }
}
