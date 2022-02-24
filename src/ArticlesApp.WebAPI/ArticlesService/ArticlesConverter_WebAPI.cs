using ArticlesApp.Core.Entities.Article;
using ArticlesApp.WebAPI.Models;
using AutoMapper;



namespace ArticlesApp.WebAPI.ArticlesService;



public class ArticlesConverter_WebAPI
{
    private readonly IMapper _mapper;

    public ArticlesConverter_WebAPI(IMapper mapper)
    {
        _mapper = mapper;
    }



    public IEnumerable<Article_WebAPI> ToEntities<T>(IEnumerable<Article> fromEntitites)
        where T : Article_WebAPI
    {
        IEnumerable<Article_WebAPI> toEntities = fromEntitites
            .Select((article) => ToEntity<Article_WebAPI>(article));

        return toEntities;
    }



    public Article ToEntity<T>(Article_WebAPI fromEntity)
            where T : Article
    {
        return _mapper.Map<Article_WebAPI, Article>(fromEntity);
    }



    public Article_WebAPI ToEntity<T>(Article fromEntity)
        where T : Article_WebAPI
    {
        return _mapper.Map<Article, Article_WebAPI>(fromEntity);
    }
}