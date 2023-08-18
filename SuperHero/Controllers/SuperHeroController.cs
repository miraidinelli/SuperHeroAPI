using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperHero.DTO;
using SuperHero.Models;

namespace SuperHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public SuperHeroController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarHerois()
        {
            var hero = await _dataContext.SuperHeroes.ToListAsync();
            return Ok(hero);
        }

        private static List<SuperHeroModel> heroes = new List<SuperHeroModel>
            {
                new SuperHeroModel {
                    Id = 1,
                    Name = "Homem Aranha",
                    FirstName = "Peter",
                    LastName = "Parker",
                    Place = "Nova Yotk"
                },
                new SuperHeroModel {
                    Id = 2,
                    Name = "Homem de ferro",
                    FirstName = "Tony",
                    LastName = "Stark",
                    Place = "Nova Yotk"
                }
        };
        private static int i = 1;

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHeroModel>> BuscarHeroiPorId(int id)
        {
            var hero = await _dataContext.SuperHeroes.FirstOrDefaultAsync(h => h.Id == id);
            if (hero == null)
                return BadRequest("Hero Not Found");
            return Ok(hero);

        }
        [HttpGet("cidade/{cidade}")]
        public async Task<ActionResult<List<SuperHeroModel>>> BuscarHeroisPorCidade(string cidade)
        {
            var hero = await _dataContext.SuperHeroes.Where(h => h.Place == cidade).ToListAsync();
            if (hero == null)
                return BadRequest("Hero Not Found");
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHeroModel>>> AdicionarHeroi(SuperHeroModel hero)
        {
            Console.WriteLine(hero.Id);
            _dataContext.SuperHeroes.Add(hero);
            await _dataContext.SaveChangesAsync();
            return Ok(hero);
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHeroModel>>> AtualizarHeroi(SuperHeroDTO request)
        {
            var dbHero = await _dataContext.SuperHeroes.FirstOrDefaultAsync(h => h.Id == request.Id);
            if(dbHero == null)
            {
                return BadRequest("Hero not Found");
                dbHero.FirstName = request.FirstName;
                dbHero.LastName = request.LastName;
                dbHero.Place = request.Place;
            }
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarHeroi(int id)
        {
            var dbHero = await _dataContext.SuperHeroes.FindAsync(id);
            if (dbHero == null)
                return BadRequest("Hero Not Found");
            _dataContext.SuperHeroes.Remove(dbHero);
            await _dataContext.SaveChangesAsync();
            return Ok("Herói excluido com sucesso");
        }
    }
}
