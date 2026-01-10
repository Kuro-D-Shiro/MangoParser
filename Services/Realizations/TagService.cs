using MangoParser.Data.DB;
using MangoParser.Data.DTO.ParserDTOs;
using MangoParser.Data.Models;
using MangoParser.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MangoParser.Services.Realizations
{
    public class TagService : ITagService
    {
        private readonly MangoParserDbContext _dbContext;

        public TagService(IDbContextFactory<MangoParserDbContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task<Tag> CreateTagFromParser(TagFromParsingDTO dto)
        {
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(x => x.Name == dto.Name);

            if (tag is not null)
                return tag;

            tag = new Tag
            {
                Name = dto.Name
            };
            _dbContext.Tags.Add(tag);

            await _dbContext.SaveChangesAsync();

            return tag;
        }
    }
}
