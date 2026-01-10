using MangoParser.Data.DB;
using MangoParser.Data.DTO.ParserDTOs;
using MangoParser.Data.DTO.ResponseDTOs;
using MangoParser.Data.Models;
using MangoParser.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MangoParser.Services.Realizations
{
    public class MangaService : IMangaService
    {
        private readonly MangoParserDbContext _dbContext;

        public MangaService(IDbContextFactory<MangoParserDbContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }

        private async Task<List<Tag>> GetOrCreateTagsAsync(IEnumerable<TagFromParsingDTO> dtos)
        {
            var names = dtos.Select(t => t.Name).ToList();
            var existing = await _dbContext.Tags.Where(t => names.Contains(t.Name)).ToListAsync();

            var result = new List<Tag>(existing);

            foreach (var dto in dtos)
            {
                if (!existing.Any(t => t.Name == dto.Name))
                {
                    var tag = new Tag { Name = dto.Name };
                    _dbContext.Tags.Add(tag);
                    result.Add(tag);
                }
            }

            return result;
        }

        private async Task<List<Genre>> GetOrCreateGenresAsync(IEnumerable<GenreFromParsingDTO> dtos)
        {
            var names = dtos.Select(g => g.Name).ToList();
            var existing = await _dbContext.Genres.Where(g => names.Contains(g.Name)).ToListAsync();

            var result = new List<Genre>(existing);

            foreach (var dto in dtos)
            {
                if (!existing.Any(g => g.Name == dto.Name))
                {
                    var genre = new Genre { Name = dto.Name };
                    _dbContext.Genres.Add(genre);
                    result.Add(genre);
                }
            }

            return result;
        }

        public async Task CreateOrUpdateMangaFromParser(MangaFromParsingDTO dto)
        {
            var manga = await _dbContext.Mangas
                .Include(m => m.Rating)
                .Include(m => m.Tags)
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Name == dto.Name);

            if (manga == null)
            {
                manga = new Manga
                {
                    Name = dto.Name,
                    EnName = dto.Eng_Name,
                    RuName = dto.Rus_Name,
                    Description = dto.Summary,
                    LastCheckout = DateTime.UtcNow,
                    Rating = new Rating
                    {
                        AverageRating = dto.Rating.Average,
                        VotesCount = dto.Rating.Votes
                    },
                    Tags = await GetOrCreateTagsAsync(dto.Tags),
                    Genres = await GetOrCreateGenresAsync(dto.Genres)
                };
                await _dbContext.Mangas.AddAsync(manga);
            }
            else
            {
                manga.Name = dto.Name;
                manga.EnName = dto.Eng_Name;
                manga.RuName = dto.Rus_Name;
                manga.Description = dto.Summary;
                manga.LastCheckout = DateTime.UtcNow;

                if (manga.Rating == null)
                    manga.Rating = new Rating { AverageRating = dto.Rating.Average, VotesCount = dto.Rating.Votes };
                else
                {
                    manga.Rating.AverageRating = dto.Rating.Average;
                    manga.Rating.VotesCount = dto.Rating.Votes;
                }

                manga.Tags = await GetOrCreateTagsAsync(dto.Tags);
                manga.Genres = await GetOrCreateGenresAsync(dto.Genres);
            }

            await _dbContext.SaveChangesAsync();
        }


        /*public async Task CreateMangaFromParser(MangaFromParsingDTO dto)
        {
            var manga = await _dbContext.Mangas
                .Include(x => x.Rating)
                .FirstOrDefaultAsync(x => x.Name == dto.Name);

            if (manga is null)
            {
                manga = new Manga
                {
                    Name = dto.Name,
                    EnName = dto.Eng_Name,
                    RuName = dto.Rus_Name,
                    Description = dto.Summary,
                    LastCheckout = DateTime.UtcNow,
                    Rating = new Rating
                    {
                        AverageRating = dto.Rating.Average,
                        VotesCount = dto.Rating.Votes
                    },
                };

                foreach (var tagName in dto.Tags)
                {
                    var tag = await _tagService.CreateTagFromParser(tagName);
                    manga.Tags.Add(tag);
                }

                foreach (var genreName in dto.Genres)
                {
                    var genre = await _genreService.CreateGenreFromParser(genreName);
                    manga.Genres.Add(genre);
                }

                await _dbContext.AddAsync(manga);
            }
            else
            {
                manga.Name = dto.Name;
                manga.EnName = dto.Eng_Name;
                manga.RuName = dto.Rus_Name;
                manga.Description = dto.Summary;
                manga.LastCheckout = DateTime.UtcNow;

                manga.Rating.AverageRating = dto.Rating.Average;
                manga.Rating.VotesCount = dto.Rating.Votes;
            }

            await _dbContext.SaveChangesAsync();
        }*/

        public async Task<IReadOnlyCollection<TrainingDataSetDTO>> GetTrainingDataSet()
        {
            var dataSet = await _dbContext.Mangas
                .Select(x => new TrainingDataSetDTO
                {
                    Tags = x.Tags.Select(t => t.Name),
                    Genres = x.Genres.Select(g => g.Name),
                    Rating = new RatingWithoutIdDTO(x.Rating)
                }).ToListAsync();

            return dataSet;
        }
    }
}
