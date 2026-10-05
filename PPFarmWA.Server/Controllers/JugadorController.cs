[HttpGet]
public async Task<ActionResult<IEnumerable<JugadorDTO>>> Get()
{
    var jugadores = await _repositorio.GetAllAsync();

    return Ok(jugadores.Select(Proyectar));
}