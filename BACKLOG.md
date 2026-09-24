# PP Farm — Backlog de Sprints 3 y 4 (Trello)

> **Producto:** Juego de gestión de huerta con minado de criptomonedas (PP Coins).
> **Stack:** Blazor Web App (.NET 10) — Server + WebAssembly, ASP.NET Core Web API, EF Core + SQL Server.
> **Equipo:** 6 programadores (1 Product Owner + 5 devs) — integrante del equipo Ginsburgo (PO) asiste + integra + demo.
> **Documento base:** "2026 - G3 - Crimson Studio.docx".
> **Periodo planificado:** Sprint 3 y Sprint 4.

---

## Etiquetas sugeridas para el tablero

| Etiqueta     | Color sugerido | Uso                                   |
|--------------|----------------|----------------------------------------|
| Backend      | Azul           | Controllers, servicios, lógica server  |
| Frontend     | Verde          | .razor, CSS, estados de UI             |
| Base de Datos| Morado         | Entidades, migraciones, seed           |
| Juego        | Naranja        | Lógica de juego (cultivo, huerta, XP)  |
| Seguridad    | Rojo           | Auth, moneda, validación, CORS         |
| QA           | Gris           | Pruebas, demo, cierre de sprint        |
| Sprint-3     | Amarillo       | Alcance del Sprint 3                   |
| Sprint-4     | Amarillo       | Alcance del Sprint 4                   |

---

## Resumen técnico y arquitectura

### Arquitectura por capas (recomendada)

```
Client (WASM/Server)  ──HTTP──▶  Server/Controllers  ──▶  Servicios (negocio)  ──▶  Repositorio  ──▶  BD (EF Core)
```

| Capa      | Proyecto             | Responsabilidad                                        | Estado |
|-----------|----------------------|--------------------------------------------------------|--------|
| Datos     | `PPFarmWA.BD`        | Entidades, `AppDbContext`, migraciones                 | ✅ Hecho |
| Acceso    | `PPFarmWA.Repositorio`| Repos de lectura/escritura genéricos y específicos     | ✅ Hecho |
| Negocio   | `PPFarmWA.Servicios` | Lógica de negocio (hoy vacío, se debe componer)        | ⚠️ Crear |
| API       | `PPFarmWA.Server`    | Controllers, auth, middleware                          | ⚠️ Completar (sin auth) |
| Cliente   | `PPFramWA.Client`    | Páginas, componentes, servicios HTTP, dominios de juego| ⚠️ Conectar al backend |

### Estado actual detectado en código

- **Controllers entregados:** `Jugador`, `Recurso`, `Item`, `Venta`, `Compra` (Swagger + DI registrada).
- **Repositorios:** genérico `Repositorio<T>` + específicos con interfaces.
- **Cliente:** páginas Home, Login, Tienda, Inventario, Internet, Intercambios; servicios HTTP (`ApiServicio`, `RecursoServicio`, `ItemServicio`, `JugadorServicio`, `VentaServicio`); dominios `Cultivo`, `Huerta`, `Jugador`; estado `JugadorState`.
- **Proyecto `PPFarmWA.Servicios`:** está vacío — la lógica de negocio hoy vive en controllers.

### Principales desafíos técnicos

1. **Seguridad de la moneda virtual (PP Coins):** validar y descontar saldo siempre en servidor, operaciones transaccionales, tipos numéricos exactos (migrar `double` → entero en centavos) y tabla de auditoría/ledger. Nunca confiar en el estado del cliente (hay jugadores hardcodeados: `Home` usa `Id=20`, `Inventario` usa `Id=2`).
2. **Sincronización multijugador:** mercado de intercambios con concurrencia (2 jugadores por misma publicación), optimistic concurrency (`rowversion`) y actualización en vivo (SignalR opcional).
3. **Conversión points → PP Coins:** tasa configurable en servidor, idempotencia anti dobles envíos y registro auditable. La evolución a "criptomoneda real" arrastra temas legales/AML: fuera de alcance del sprint, riesgo a acordar.
4. **Autenticación y permisos:** hoy `password` en **texto plano** y expuesta en `JugadorDTO`/respuestas API. Falta login/registro real, hashing y roles (`esAdmin`, `esTienda`).
5. **Deuda de integración:** botones "Comprar", "Vender", "Equipar" y "Convertir" son mock/no persistidos; el grueso de Sprint 4 es conectar el frontend al backend real.

---

## SPRINT 3 — Completar y cerrar

---

### S3-01 · Verificar y documentar controllers, repos e interfaces

**Descripción:** Auditar los 5 controllers y 4 repositories entregados: cobertura de endpoints, códigos HTTP coherentes (200/400/404), DI registrada en `Program.cs`. Documentar la API.

**Criterios de aceptación:**
- [ ] Swagger lista todos los endpoints del server.
- [ ] Cada controller devuelve códigos correctos ante casos válidos e inválidos.
- [ ] La DI registra todos los repositorios sin errores de arranque.
- [ ] PR revisado por un compañero distinto del autor.

**Etiquetas:** Backend · Sprint-3

---

### S3-02 · Seed de catálogo de recursos de tienda

**Descripción:** Crear clase `Seeder` (invocada desde `Program.cs`) que inserte recursos con `deTienda = true` (nombre, descripción, eficiencia, durabilidad, valor, tipo, rareza) y jugadores de prueba si hace falta.

**Criterios de aceptación:**
- [ ] Al iniciar la app, la tienda muestra recursos sin inserción manual en SQL.
- [ ] El seed es idempotente (no duplica datos al reiniciar).
- [ ] Incluye al menos un ítem por tipo (Herramienta, Dispositivo, Potenciador) y por rareza.

**Etiquetas:** Base de Datos · Backend · Sprint-3

---

### S3-03 · Login: formulario HTML maquetado

**Descripción:** Página `/login` con inputs y botones "Iniciar sesión" / "Crear cuenta". Corregir typos ("usiario" → "usuario", "seción" → "sesión", "Iniciar seción" → "Iniciar sesión") y mantenerla desacoplada del backend.

**Criterios de aceptación:**
- [ ] La página navega sin errores de consola.
- [ ] Los campos requieren valores (validación requerida en el form).
- [ ] Los textas visibles no tienen errores ortográficos.

**Etiquetas:** Frontend · Sprint-3

---

### S3-04 · Home + componentes de lógica de juego

**Descripción:** Auditar `CultivoComponent`, `HuertaCeldasComponent`, `Cultivo`, `Huerta` y `JugadorState`: cosechar suma XP/puntos, sube de nivel, recalcula celdas disponibles y notifica cambios a la UI.

**Criterios de aceptación:**
- [ ] Cosechar un cultivo suma XP y puntos al jugador.
- [ ] Al alcanzar el umbral se sube de nivel y se actualiza la cantidad de celdas.
- [ ] El nivel/XP se recalculan correctamente según la fórmula actual.
- [ ] Sin excepciones ni valores negativos en saldos.

**Etiquetas:** Juego · Frontend · Sprint-3

---

## SPRINT 4 — Conectar el frontend al backend y cerrar el MVP

---

### FASE A · Autenticación y sesión

**S4-01 · Endpoint de registro (POST /api/auth/registro)**

**Descripción:** Validar `userName` (mín. 3), `email` y `password` (mín. 5); crear `Jugador` con password hasheada; inicializar `ppCoins = 100`; devolver token de sesión. El `JugadorDTO` de respuesta no debe contener la password.

**Criterios de aceptación:**
- [ ] Crear usuario vía Swagger devuelve 201 + token.
- [ ] Usuario o email duplicado devuelve 409 con mensaje claro.
- [ ] La password no se almacena en texto plano (hash verificado en BD).

**Etiquetas:** Backend · Seguridad · Sprint-4

---

**S4-02 · Endpoint de login (POST /api/auth/login)**

**Descripción:** Autenticar credenciales, emitir JWT/cookie con claims (`Id`, `esAdmin`, `esTienda`).

**Criterios de aceptación:**
- [ ] Credenciales válidas → 200 + token válido.
- [ ] Credenciales inválidas → 401.
- [ ] El token expira y los accesos vencidos se rechazan.

**Etiquetas:** Backend · Seguridad · Sprint-4

---

**S4-03 · Proteger endpoints y roles**

**Descripción:** Aplicar `[Authorize]` sobre mutaciones (compra, venta, conversión, mercado) y checks de rol donde corresponda.

**Criterios de aceptación:**
- [ ] Peticiones sin token a endpoints protegidos → 401.
- [ ] `esAdmin` puede crear/editar recursos; un jugador normal no.
- [ ] `esTienda` puede listar el catálogo de tienda.

**Etiquetas:** Backend · Seguridad · Sprint-4

---

**S4-04 · Eliminar exposición de password**

**Descripción:** Quitar `password` del `JugadorDTO` y de todas las proyecciones de controllers; `JugadorController.Post` recibe un `RegistroDTO` separado.

**Criterios de aceptación:**
- [ ] Ninguna respuesta de `api/jugador` contiene el campo password (verificado por prueba).
- [ ] No hay referencias a `password` en DTOs de respuesta.

**Etiquetas:** Backend · Seguridad · Sprint-4

---

### FASE B · Frontend conectado

**S4-05 · Conectar Login/Registro (frontend)**

**Descripción:** Reemplazar el HTML estático de `Login.razor`: formulario para login y registro, mostrar errores, guardar token (localStorage/cookie) y redirigir a `/`.

**Criterios de aceptación:**
- [ ] Login y registro reales funcionan de punta a punta desde el navegador.
- [ ] Los errores de credenciales se muestran al usuario.
- [ ] No se guarda password en el cliente.
- [ ] Tras login con éxito se navega a la página principal.

**Etiquetas:** Frontend · Seguridad · Sprint-4

---

**S4-06 · Home con jugador real (quitar hardcode)**

**Descripción:** Sustituir el `new Jugador(){Id=20,...}` de `Home.razor` por la carga del jugador desde la sesión/token (`JugadorServicio.ObtenerPorId`) y persistir XP/puntos/coins al server.

**Criterios de aceptación:**
- [ ] Al recargar la página, nivel/XP/PP Coins/points del jugador autenticado se recuperan de la BD.
- [ ] Sin jugador autenticado, se redirige a `/login`.
- [ ] XP y points ganados al cosechar se persisten.

**Etiquetas:** Frontend · Juego · Sprint-4

---

**S4-07 · Tienda: conectar botón "Comprar"**

**Descripción:** Usar el `CompraController` existente desde `Tienda.razor` (cantidad 1–3), feedback de éxito/error y refresco de saldo + inventario.

**Criterios de aceptación:**
- [ ] Comprar descuenta PP Coins y agrega/actualiza el `Item`.
- [ ] Compra sin saldo → mensaje de error, sin crear ítem.
- [ ] Cantidad fuera de rango (1–3) se valida en la UI y en servidor.
- [ ] El saldo y la cantidad en inventario se actualizan tras comprar.

**Etiquetas:** Frontend · Backend · Sprint-4

---

**S4-08 · Inventario: "Equipar" y "Vender" reales**

**Descripción:** "Equipar" persiste la herramienta seleccionada (`idUltimaHerramienta` en `Jugador`, hoy es estado en memoria); "Vender" llama al flujo de venta contra `VentaController` y acredita PP Coins al jugador.

**Criterios de aceptación:**
- [ ] Equipar y recargar: la herramienta sigue equipada y Home la muestra.
- [ ] Vender reduce stock y suma PP Coins.
- [ ] Con stock en 0 no se puede vender.

**Etiquetas:** Frontend · Backend · Sprint-4

---

### FASE C · Moneda y conversión

**S4-09 · Servicio de conversión points → PP Coins**

**Descripción:** `ConversorServicio` con tasa configurable (appsettings), operación transaccional que valida saldo de points, convierte, descuenta, acredita coins y registra en tabla `Transaccion` (auditoría). Proteger contra dobles envíos (idempotencia).

**Criterios de aceptación:**
- [ ] Convertir descuenta points y acredita coins según la tasa.
- [ ] Cada conversión queda registrada en auditoría.
- [ ] Un segundo request duplicado no duplica la conversión.
- [ ] Points insuficientes → 400 sin efectos parciales.

**Etiquetas:** Backend · Seguridad · Sprint-4

---

**S4-10 · Componente de conversión (página de computadora)**

**Descripción:** Conectar `Internet.razor` (vista "Convertir"): input de points → equivalente calculado en PP Coins, botón "Confirmar Conversión", recarga de saldos y listado de conversiones realizadas.

**Criterios de aceptación:**
- [ ] El equivalente se calcula y confirma contra el backend.
- [ ] Tras confirmar, points/coins se actualizan en pantalla.
- [ ] Se muestran las conversiones recientes del jugador.

**Etiquetas:** Frontend · Sprint-4

---

**S4-11 · Saldos seguros: tipo entero + ledger de transacciones**

**Descripción:** Migrar `double ppCoins` (y valores monetarios) a representación entera (centavos); crear tabla `Transaccion` (`JugadorId`, tipo, monto, fecha); envolver compras/conversiones/ventas en transacciones EF con `SaveChangesAsync` atómico.

**Criterios de aceptación:**
- [ ] No hay operación monetaria sin registro en `Transaccion`.
- [ ] Un fallo a mitad de compra no descuenta coins ni crea ítem.
- [ ] La migración y los datos existentes se migran sin pérdida.

**Etiquetas:** Base de Datos · Seguridad · Sprint-4

---

### FASE D · Mercado de intercambios multijugador

**S4-12 · Modelo de publicación de venta (listing)**

**Descripción:** Extender `Venta` (o nueva entidad `Publicacion`) con ítem/recurso + cantidad, precio, `estado` (activa/cerrada), fecha de publicación y `rowversion` para concurrencia. Nueva migración EF Core.

**Criterios de aceptación:**
- [ ] La migración aplica correctamente.
- [ ] El esquema soporta publicaciones activas y cerradas con control de versión.
- [ ] Se preserva la relación con inventario (Item) del jugador.

**Etiquetas:** Base de Datos · Sprint-4

---

**S4-13 · Endpoints de mercado (publicar / listar / comprar)**

**Descripción:** Publicar listing desde el inventario; listar publicaciones activas excluyendo las propias; comprar publicación de forma transaccional (validar stock → transferir coins → mover Item → marcar cerrada) con manejo de conflicto de concurrencia.

**Criterios de aceptación:**
- [ ] Dos clientes compran el mismo listing: solo uno lo obtiene, el otro recibe 409.
- [ ] El vendedor recibe las coins al confirmarse la compra.
- [ ] Las publicaciones compradas desaparecen de la lista activa.

**Etiquetas:** Backend · Base de Datos · Sprint-4

---

**S4-14 · Intercambios.razor conectado al backend**

**Descripción:** Reemplazar el mock estático: publicar intercambio desde los ítems del jugador, listar el mercado real, botón "Comprar" funcional y estados de carga/vacío/error. Mover estilos del `.razor` a `.razor.css`.

**Criterios de aceptación:**
- [ ] Un jugador publica, otro ve y compra, y ambos ven coins/ítems actualizados.
- [ ] Estados de carga, vacío y error visibles.
- [ ] Ya no hay estilos inline en el componente.

**Etiquetas:** Frontend · Sprint-4

---

**S4-15 · Página de computadora (vista Intercambiar) integrada**

**Descripción:** En `Internet.razor`, la vista "Intercambiar" consume el mismo endpoint del mercado que `Intercambios.razor` (o redirige a `/intercambios`), evitando duplicación de lógica.

**Criterios de aceptación:**
- [ ] Ambas rutas muestran el mismo mercado real.
- [ ] Las operaciones de compra/publicación funcionan desde ambas vistas.

**Etiquetas:** Frontend · Sprint-4

---

### FASE E · Calidad y cierre

**S4-16 · Validación de entrada y manejo global de errores**

**Descripción:** DataAnnotations/validación en DTOs, middleware global de excepciones, CORS correcto y rate-limiting en endpoints sensibles (compra, conversión).

**Criterios de aceptación:**
- [ ] Requests malformados → 400 con mensaje legible.
- [ ] Errores inesperados no exponen stack traces.
- [ ] Rate-limiting activo en compra y conversión.

**Etiquetas:** Backend · Seguridad · Sprint-4

---

**S4-17 · Pruebas end-to-end del flujo del jugador**

**Descripción:** Script manual/automatizado del recorrido completo: registro → login → cosechar/subir de nivel → comprar en tienda → equipar → convertir points → publicar y comprar intercambio.

**Criterios de aceptación:**
- [ ] El flujo completo funciona de punta a punta.
- [ ] Sin errores de consola ni estado inconsistente tras recargar.
- [ ] Documentado el procedimiento de prueba.

**Etiquetas:** QA · Sprint-4

---

**S4-18 · Demo de Sprint Review**

**Descripción:** Preparar escenario de demo (datos seed, 2 jugadores en el mercado) y checklist de presentación ante cliente/PO.

**Criterios de aceptación:**
- [ ] La demo se ejecuta sin fallas.
- [ ] Escenario de mercado multijugador preparado (2 jugadores).

**Etiquetas:** QA · Sprint-4

---

## Flujo de trabajo — 3 recomendaciones

### 1. Repartir por slices verticales, no por capa

Cada programador es dueño de una feature completa (frontend + backend propios), evitando que todos toquen los mismos archivos y reduciendo conflictos de merge.

| Programador | Slice vertical | Tarjetas |
|-------------|----------------|----------|
| Dev 1 | Autenticación y sesión | S4-01, S4-02, S4-03, S4-04, S4-05 |
| Dev 2 | Tienda y catálogo | S3-02, S4-07 |
| Dev 3 | Inventario y equipamiento | S4-08 |
| Dev 4 | Moneda y conversión | S4-09, S4-10, S4-11 |
| Dev 5 | Mercado backend | S4-12, S4-13 |
| Dev 6 (PO) | Mercado frontend + Home real + integración/demo | S4-06, S4-14, S4-15, S4-18 |

El PO además convoca integraciones, coordina dependencias y prepara la demo. Las tarjetas transversales (S3-01, S3-03, S3-04, S4-16, S4-17) se asignan al dev con menor carga en cada semana o rotan.

### 2. Fechas de inicio/fin obligatorias y WIP limitado

- **Trello:** `Due date` = fecha de fin acordada; Power-Up *Custom Fields* con campo "Fecha inicio"; marcar la fecha real al pasar la tarjeta a "En Progreso".
- **Columnas:** `Backlog → En Progreso (máx. 1 tarjeta por persona) → En Review → Listo`.
- **Definition of Done:** la tarjeta llega a "Listo" solo cuando cada criterio de aceptación está marcado en la checklist de la tarjeta.
- Al asignar una tarjeta, registrar **fecha de inicio** y **fecha de fin estimada** en la descripción para el seguimiento en Sprint Review.

### 3. Comunicación y revisión por pares

- **Daily corto (10 min)** o async por chat respondiendo: *¿qué entrego hoy, qué me bloquea, qué necesita de otro dev?*
- **Revisión cruzada:** la tarjeta pasa de "En Progreso" a "En Review" únicamente con PR y un reviewer **distinto del autor** (rotar: cada dev revisa la vertical del siguiente en la tabla).
- Los bloqueos de dependencia (p. ej. S4-13 → S4-14) se avisan el mismo día; nunca se dejan para el Sprint Review.