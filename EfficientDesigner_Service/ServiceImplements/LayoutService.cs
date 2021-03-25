using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Service.Contexts;
using EfficientDesigner_Service.Models;
using EfficientDesigner_Service.Services;
using Microsoft.EntityFrameworkCore;

namespace EfficientDesigner_Service.ServiceImplements
{
    public class LayoutService : ILayoutService
    {
        public async Task<Layout[]> GetLayouts()
        {
            using (var context = new LayoutContext())
            {
                return await context.Layouts.OrderByDescending(x => x.CreateTime).Include(x => x.PropertyBindings).ToArrayAsync();
                //return await context.Layouts.OrderByDescending(x => x.CreateTime).ToArrayAsync();
            }
        }

        public Layout UpdateLayout(Layout layout)
        {
            using (var context = new LayoutContext())
            {
                if (context.Layouts.Any(x => x.LayoutId == layout.LayoutId))
                {
                    context.Layouts.Update(layout);
                }
                else
                {
                    layout = context.Layouts.Add(layout).Entity;
                }
                context.SaveChanges();
                return layout;
            }
        }

        public void RemoveLayout(Layout layout)
        {
            using (var context = new LayoutContext())
            {
                context.Layouts.Remove(layout);
                context.SaveChanges();
            }
        }

        public async Task<DataSource[]> GetDataSource()
        {
            using (var context = new LayoutContext())
            {
                return await context.DataSources.OrderByDescending(x => x.CreateTime).ToArrayAsync();
            }
        }

        public DataSource UpdateDataSource(DataSource dataSource)
        {
            using (var context = new LayoutContext())
            {
                if (context.DataSources.Any(x => x.DataSourceId == dataSource.DataSourceId))
                {
                    context.Update(dataSource);
                }
                else
                {
                    dataSource = context.Add(dataSource).Entity;
                }

                context.SaveChanges();
                return dataSource;
            }
        }

        public async Task<ServiceInfo[]> UpdateServiceInfos(bool returnResult, params ServiceInfo[] serviceInfos)
        {
            if (serviceInfos == null || serviceInfos.Length == 0) return Array.Empty<ServiceInfo>();
            var results = new List<ServiceInfo>();

            using (var context = new LayoutContext())
            {
                var list1 = serviceInfos.Intersect(context.ServiceInfos).ToArray();
                var list2 = serviceInfos.Except(list1);
                foreach (var info in list1)
                {
                    context.Update(info);
                }

                results.AddRange(list2.Select(info => context.Add(info).Entity).Where(result => returnResult));
                
                context.SaveChanges();
                return results.ToArray();
            }
        }

        public async Task<ServiceInfo[]> GetServiceInfos(string name = null)
        {
            using (var context = new LayoutContext())
            {
                return string.IsNullOrWhiteSpace(name) ? await context.ServiceInfos.OrderBy(x => x.Name).ToArrayAsync() : await context.ServiceInfos.Where(x => x.Name == name).ToArrayAsync();
            }
        }
    }
}
