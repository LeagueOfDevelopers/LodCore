using Dapper;
using LodCore.Domain.ProjectManagment;
using LodCore.QueryService.DTOs;
using LodCore.QueryService.Views;
using LodCore.QueryService.Views.ProjectView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCore.QueryService.Queries.ProjectQuery
{
    public class GetSomeProjectsQuery : IQuery<SomeProjectsView>
    {
        public GetSomeProjectsQuery(int offset, int count, int[] categories)
        {
            Offset = offset;
            Count = count;
            Categories = categories;
            BDCategories = CategoryOffset();
            Sql = "SELECT * FROM projects AS Project " +
                  "LEFT JOIN projectTypes AS ProjectType ON Project.ProjectId = ProjectType.project_key " +
                 $"WHERE id IN ({string.Join(",", BDCategories)});";
        }

        public SomeProjectsView FormResult(List<ProjectDto> rawResult)
        {
            return new SomeProjectsView(rawResult, rawResult.Count());
        }
        //Смещает номера категорий для работы с бд
        private int[] CategoryOffset()
        {
            int[] newCategories = new int[Categories.Length];
            Categories.CopyTo(newCategories, 0);

            for(int i =0; i < newCategories.Length; i++)
            {
                if (newCategories[i] == 0)
                {
                    newCategories = ((int[])Enum.GetValues(typeof(ProjectType)))
                        .Skip(1)
                        .Select(el => el - 1)
                        .ToArray();
                    return newCategories;
                }
                newCategories[i]--;
            }
            return newCategories;
        }
        
        public int Offset { get; }
        public int Count { get; }
        public int[] Categories { get; private set; }
        public string Sql { get; }
        private int[] BDCategories;
    }
}
