using System;
using System.Collections.Generic;
using System.Linq;
using LodCore.Domain.ProjectManagment;
using LodCore.QueryService.DTOs;
using LodCore.QueryService.Views.ProjectView;

namespace LodCore.QueryService.Queries.ProjectQuery
{
    public class GetSomeProjectsQuery : IQuery<SomeProjectsView>
    {
        private readonly int[] BDCategories;

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

        public int Offset { get; }
        public int Count { get; }
        public int[] Categories { get; }
        public string Sql { get; }

        public SomeProjectsView FormResult(List<ProjectDto> rawResult)
        {
            return new SomeProjectsView(rawResult, rawResult.Count());
        }

        //Смещает номера категорий для работы с бд
        private int[] CategoryOffset()
        {
            var newCategories = new int[Categories.Length];
            Categories.CopyTo(newCategories, 0);

            for (var i = 0; i < newCategories.Length; i++)
            {
                if (newCategories[i] == 0)
                {
                    newCategories = ((int[]) Enum.GetValues(typeof(ProjectType)))
                        .Skip(1)
                        .Select(el => el - 1)
                        .ToArray();
                    return newCategories;
                }

                newCategories[i]--;
            }

            return newCategories;
        }
    }
}