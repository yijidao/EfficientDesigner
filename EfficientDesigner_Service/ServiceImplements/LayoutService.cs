using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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

        public ServiceInfo[] UpdateServiceInfos(bool returnResult, params ServiceInfo[] serviceInfos)
        {
            if (serviceInfos == null || serviceInfos.Length == 0) return Array.Empty<ServiceInfo>();
            var results = new List<ServiceInfo>();

            using (var context = new LayoutContext())
            {
                foreach (var info in serviceInfos)
                {
                    var first = context.ServiceInfos.SingleOrDefault(x => x.Id != null && x.Id == info.Id);
                    if (first == null)
                    {
                        if (returnResult)
                        {
                            results.Add(context.Add(info).Entity);
                        }
                        else
                        {
                            context.Add(info);
                        }
                    }
                    else
                    {
                        first.Address = info.Address;
                        first.Enable = info.Enable;
                        first.Function = info.Function;
                        first.Service = info.Service;
                        first.Name = info.Name;
                        if (returnResult)
                        {
                            results.Add(first);
                        }
                    }
                }
                context.SaveChanges();
                return results.ToArray();
            }
        }

        public async Task<ServiceInfo[]> GetServiceInfos(params string[] names)
        {
            names = names.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            using (var context = new LayoutContext())
            {
                return names.Length == 0 ? await context.ServiceInfos.OrderBy(x => x.Name).ToArrayAsync() : await context.ServiceInfos.Where(x => names.Contains(x.Name)).ToArrayAsync();
            }
        }

        public async Task<ServiceInfo[]> GetServiceInfos(Expression<Func<ServiceInfo, bool>> predicate)
        {
            using (var context = new LayoutContext())
            {
                return await context.ServiceInfos.Where(predicate).OrderBy(x => x.Name).ToArrayAsync();
            }
        }

        public void RemoveServiceInfosFor(params string[] names)
        {
            using (var context = new LayoutContext())
            {
                context.ServiceInfos.RemoveRange(context.ServiceInfos.Where(x => names.Contains(x.Name)));
                context.SaveChanges();
            }
        }
    }
}
