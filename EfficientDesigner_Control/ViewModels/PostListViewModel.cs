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
        private ObservableCollection<PostItemViewModel> _postItems;
        public ObservableCollection<PostItemViewModel> PostItems
        {
            get => _postItems;
            set => SetProperty(ref _postItems, value);
        }

        public PostListViewModel()
        {

            PostItems = new ObservableCollection<PostItemViewModel>
            {
                new PostItemViewModel("标题1"),
                new PostItemViewModel("标题2"),
                new PostItemViewModel("标题3"),
            };

        }
    }
}
