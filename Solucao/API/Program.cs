using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

///COLOCAR TUDO ISSO ATÉ AS SETAS APRA FZR A LIGACAO DA API COM O FRONT, NO FINAL COLOCAR AQUELE TBM
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddCors(options =>
    options.AddPolicy("Acesso Total",
        configs => configs
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod())
);

var app = builder.Build();
///////


app.MapGet("/", () => "Programa Loja Estudo");

app.MapPost("/api/cliente/cadastrar", ([FromBody] Cliente cliente, [FromServices] AppDbContext ctx)=>{

ctx.Clientes.Add(cliente);
ctx.SaveChanges();
return Results.Created("",cliente);
});

app.MapPost("/api/produto/cadastrar", ([FromBody] Produto produto, [FromServices] AppDbContext ctx)=>{

    ctx.Produtos.Add(produto);
    ctx.SaveChanges();
    return Results.Created("",produto);

});

app.MapGet("/api/cliente/listarTudo", ([FromServices] AppDbContext ctx) =>{

if(ctx.Clientes.Count() > 0){
    return Results.Ok(ctx.Clientes.ToList());
}
else{
    return Results.NotFound();
}
});

app.MapGet("/api/produto/listarTudo", ([FromServices] AppDbContext ctx) =>{
    if(ctx.Produtos.Count() > 0){
        return Results.Ok(ctx.Produtos.ToList());
    }
    else{
        return Results.NotFound();
    }
});

app.MapGet("/api/cliente/buscar/id:{id}", ([FromRoute] int id, [FromServices] AppDbContext ctx) =>{


    ///PARA ENCONTRAR ALGUMA COISA EU TENHO QUE CRIAR UMA VARIAVEL cliente APARTIR DA CLASSE CLIENTE
    ///ESSE cliente VAI SER IGUAL AO CLIENTE QUE ESTA NO BANCO DE DADOS QUE POSSUI ESSE ID QUE ESTOU BUSCANDO NO HTTP
    ///E NA HORA DE RETORNAR O RESULTADO, EU MOSTRO APENAS ESSE cliente EM ESPECIFICO
    
    Cliente? cliente = ctx.Clientes.Find(id);

    if(cliente == null){
        return Results.NotFound("O Cliente não esta cadastrado no sistema");
    }

    return Results.Ok(cliente);


});

app.MapGet("/api/produto/buscar/id:{id}", ([FromRoute] int id, [FromServices] AppDbContext ctx) =>{

    Produto? produto = ctx.Produtos.Find(id);

    if(produto == null){
        return Results.NotFound("Produto não cadastrado");
    }

    return Results.Ok(produto);

});

///COLOCAR ESSE TBM
app.UseCors("Acesso Total");
app.Run();
