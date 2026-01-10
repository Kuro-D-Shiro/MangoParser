using MangoParser.Data.DB;
using MangoParser.Data.DTO.ParserDTOs;
using MangoParser.Data.Models;
using MangoParser.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MangoParser.Services.Realizations
{
    public class GenreService : IGenreService
    {
        private readonly MangoParserDbContext _dbContext;
        public GenreService(IDbContextFactory<MangoParserDbContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task<Genre> CreateGenreFromParser(GenreFromParsingDTO dto)
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Name == dto.Name);

            if (genre is not null)
                return genre;

            genre = new Genre
            {
                Name = dto.Name
            };
            _dbContext.Genres.Add(genre);

            await _dbContext.SaveChangesAsync();

            return genre;
        }
    }
}
