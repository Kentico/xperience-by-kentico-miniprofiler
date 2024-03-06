﻿
using CMS.Websites;

namespace DancingGoat.Models
{
    public record ArticlesSectionViewModel(IEnumerable<ArticleViewModel> Articles, string ArticlesPath)
        : IWebPageBasedViewModel
    {
        /// <inheritdoc/>
        public IWebPageFieldsSource WebPage { get; init; }


        /// <summary>
        /// Maps <see cref="ArticlesSection"/> to a <see cref="ArticlesSectionViewModel"/>.
        /// </summary>
        public static ArticlesSectionViewModel GetViewModel(ArticlesSection articlesSection, IEnumerable<ArticleViewModel> articles, string articlesPath) =>
            new(articles, articlesPath)
            {
                WebPage = articlesSection
            };
    }
}
