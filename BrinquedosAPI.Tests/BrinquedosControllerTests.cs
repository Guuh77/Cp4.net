using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BrinquedosAPI.Controllers;
using BrinquedosAPI.Data;
using BrinquedosAPI.Models;
using Xunit;

namespace BrinquedosAPI.Tests
{
    public class BrinquedosControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task GetBrinquedos_ReturnsAllToys()
        {
            var context = GetDbContext();
            context.Brinquedos.Add(new Brinquedo { Id_brinquedo = 1, Nome_brinquedo = "Carrinho", Tipo_brinquedo = "Veículo", Classificacao = "+3 anos", Tamanho = "Pequeno", Preco = 15.50m });
            context.Brinquedos.Add(new Brinquedo { Id_brinquedo = 2, Nome_brinquedo = "Boneca", Tipo_brinquedo = "Figura", Classificacao = "+5 anos", Tamanho = "Médio", Preco = 25.00m });
            await context.SaveChangesAsync();
            var controller = new BrinquedosController(context);

            var result = await controller.GetBrinquedos();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Brinquedo>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Brinquedo>>(actionResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task GetBrinquedo_ReturnsNotFound_WhenIdIsInvalid()
        {
            var context = GetDbContext();
            var controller = new BrinquedosController(context);

            var result = await controller.GetBrinquedo(99);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostBrinquedo_CreatesToyAndReturnsIt()
        {
            var context = GetDbContext();
            var controller = new BrinquedosController(context);
            var newToy = new Brinquedo { Id_brinquedo = 1, Nome_brinquedo = "Lego", Tipo_brinquedo = "Blocos", Classificacao = "+8 anos", Tamanho = "Grande", Preco = 150.00m };

            var result = await controller.PostBrinquedo(newToy);

            var actionResult = Assert.IsType<ActionResult<Brinquedo>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var model = Assert.IsType<Brinquedo>(createdAtActionResult.Value);
            
            Assert.Equal("Lego", model.Nome_brinquedo);
            Assert.Equal(1, context.Brinquedos.Count());
        }

        [Fact]
        public async Task PutBrinquedo_UpdatesExistingToy()
        {
            var context = GetDbContext();
            var toy = new Brinquedo { Id_brinquedo = 1, Nome_brinquedo = "Bola", Tipo_brinquedo = "Esporte", Classificacao = "+5 anos", Tamanho = "Médio", Preco = 30.00m };
            context.Brinquedos.Add(toy);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var controller = new BrinquedosController(context);
            var updatedToy = new Brinquedo { Id_brinquedo = 1, Nome_brinquedo = "Bola de Futebol", Tipo_brinquedo = "Esporte", Classificacao = "+5 anos", Tamanho = "Médio", Preco = 35.00m };

            var result = await controller.PutBrinquedo(1, updatedToy);

            Assert.IsType<NoContentResult>(result);
            var modifiedToy = await context.Brinquedos.FindAsync(1);
            Assert.Equal("Bola de Futebol", modifiedToy.Nome_brinquedo);
            Assert.Equal(35.00m, modifiedToy.Preco);
        }

        [Fact]
        public async Task DeleteBrinquedo_RemovesToyAndReturnsNoContent()
        {
            var context = GetDbContext();
            var toy = new Brinquedo { Id_brinquedo = 1, Nome_brinquedo = "Quebra-cabeça", Tipo_brinquedo = "Jogo", Classificacao = "+10 anos", Tamanho = "Grande", Preco = 45.00m };
            context.Brinquedos.Add(toy);
            await context.SaveChangesAsync();

            var controller = new BrinquedosController(context);

            var result = await controller.DeleteBrinquedo(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, context.Brinquedos.Count());
        }
    }
}
