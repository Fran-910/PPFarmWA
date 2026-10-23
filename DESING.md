# DESING.md

## PP Farm — Identidad, esencia y sistema visual

> **Producto:** PP Farm (`PPFarmWA`)
> **Género:** gestión de huerta + economía lúdica de criptomonedas
> **Plataforma:** Web — Blazor WebAssembly
> **Idioma:** español rioplatense
> **Versión del documento:** 1.0 — Octubre 2026
> **Fuentes de verdad:** implementación en `PPFramWA.Client` (`wwwroot/css`, `.razor`, `.razor.css`, assets), referencias de `DesingFiles/stitch_ppfarm_design_system` y capturas de `Capturas/`.

> **Nota sobre el nombre:** se conserva `DESING.md` tal como lo pidió el equipo. El archivo `PPFramWA.Client/DesingFiles/stitch_ppfarm_design_system/retro_farmstead_hud/DESIGN.md` queda como referencia histórica de una variante visual anterior (shell navy, tipografía Rubik). **La fuente de verdad visual de este documento es el código actual**: madera, tierra, pasto y tipografía pixel.

---

## Índice

1. [La esencia](#1-la-esencia)
2. [Identidad visual](#2-identidad-visual)
3. [Arquitectura de la interfaz](#3-arquitectura-de-la-interfaz)
4. [Componentes](#4-componentes)
5. [Pantallas y flujos](#5-pantallas-y-flujos)
6. [Economía y progresión](#6-economía-y-progresión)
7. [Accesibilidad y ergonomía](#7-accesibilidad-y-ergonomía)
8. [Implementación y gobernanza](#8-implementación-y-gobernanza)
9. [Estado actual y deuda de diseño](#9-estado-actual-y-deuda-de-diseño)
10. [Anexos](#10-anexos)

---

## 0. TL;DR

PP Farm es una huerta de 16 bits que se siente como un objeto físico: pasto animado, madera gastada, tierra, oro de feria y una terminal. El jugador golpea cultivos, cosecha XP y Points, sube de nivel, desbloquea celdas y convierte Points en PP Coins para comprar herramientas y comerciar con otros jugadores.

La interfaz **no es una página web con estética retro**: es un HUD de juego. Biseles duros, cero radios, tipografía pixel, sombras sólidas y consecuencias visibles en cada acción.

La tensión poética del producto: **lo analógico de la huerta y lo digital de la economía conviven en la misma pantalla**. Esa tensión es la identidad.

---

## 1. La esencia

### 1.1 Pitch

> Una granja de bolsillo para cultivar, cosechar y hacer crecer una economía entre granjeros. Cada clic es un golpe de herramienta; cada cosecha, un pequeño ritual de progreso; cada PP Coin, una promesa de que la paciencia se convierte en algo que se puede gastar.

Versión corta para presentaciones:

> **Stardew Valley se encuentra con una terminal de trueque de 16 bits.**

### 1.2 Fantasía central

Sos un granjero que administra dos capas del mismo mundo:

| Capa | Qué representa | Cómo se ve | Cómo se siente |
|---|---|---|---|
| **La huerta** | El mundo físico, el trabajo lento, la paciencia | Pasto ajedrezado, cerca de madera, tierra, cultivos | Táctil, cálida, manual |
| **La terminal** | La economía, la conversión, el mercado | Pantalla de computadora, pestañas, saldos, publicaciones | Precisa, sistémica, social |

El jugador no “usa una app”: **habita una granja y una terminal**. La interfaz debe sostener esa fantasía incluso en los formularios, los errores y las tablas.

### 1.3 Pilares de diseño

| Pilar | Qué significa | Cómo se ve | Cómo se rompe |
|---|---|---|---|
| **1. Tacto físico** | Cada acción se siente como tocar un objeto | Botones que se hunden, biseles, sombras duras, cursor de herramienta, golpe de cultivo | Botones planos, hover suave, bordes redondeados |
| **2. Progreso visible** | El crecimiento se ve y se celebra | HUD siempre visible, celdas que se desbloquean, skins de cultivo, popup de cosecha | Números escondidos, feedback genérico, “toast” único para todo |
| **3. Campo + terminal** | Dos lenguajes, una misma paleta | Madera y pasto para la huerta; pizarra y fósforo para la computadora | Que la computadora parezca una web blanca ajena al juego |
| **4. Economía clara y social** | El valor se entiende y se comparte | Points → PP Coins, precios, publicaciones, vendedor/comprador | Monedas ambiguas, dobles nombres, acciones sin confirmación |
| **5. Ritual de partida** | Entrar, jugar, guardar, volver | “Guardar partida”, mensaje de sincronización, sesión persistente | Guardado invisible, pérdida de estado, mensajes fríos |
| **6. Cozy con filo** | Cálido, pero no infantil | Colores de campo, humor seco, precisión de sistema | Pasteles genéricos, emojis como única iconografía, tono corporativo |

### 1.4 Lo que el juego NO es

- **No es un dashboard SaaS** con una skin verde.
- **No es fotorrealismo ni 3D.** Todo debe poder dibujarse con píxeles.
- **No es una app de finanzas real.** PP Coins son ficción lúdica; el tono no debe prometer rendimiento económico real.
- **No es un idle game pasivo.** El clic, la herramienta y el ritmo de cosecha importan.
- **No es “Bootstrap con emojis”.** La estética no es un accesorio: es el juego.
- **No es oscuro ni hostil.** La paleta es cálida y legible; el contraste viene de la madera y el oro, no del negro plano.

### 1.5 Tono y voz

La voz es **cercana, rioplatense y directa**. Habla como un tablero de juego, no como un formulario corporativo.

- Usar **vos**, no “tú”: *“Conseguí”, “Equipá”, “Vendé”, “Volvé”*.
- Frases cortas. Verbo de acción primero.
- Humor seco, nunca payasadas.
- Un signo de exclamación como máximo en un mensaje de éxito.
- Los errores explican **qué pasó y qué hacer**, sin stack traces.
- La interfaz puede ser técnica (“PP-FARM-OS”), pero nunca fría.

| Contexto | Copy aprobado | Evitar |
|---|---|---|
| Tienda | “Conseguí recursos para tu huerta.” | “Bienvenido a la tienda oficial de recursos” |
| Inventario | “Equipá o vendé tus recursos.” | “Gestione su inventario” |
| Carga | “Cargando recursos...”, “Guardando granjero...” | “Loading...”, “Please wait” |
| Éxito | “¡Compra exitosa! Se equipó en tu mochila.” | “Operación realizada satisfactoriamente!!!” |
| Error de saldo | “No tenés suficientes PP Coins.” | “Error 400: insufficient funds” |
| Conversión | “Necesitás al menos 10 Points para convertir.” | “Monto inválido” |
| Guardado | “Partida sincronizada con éxito.” | “Save complete” |
| Mercado | “Intercambio publicado correctamente.” | “Listing created” |

**Términos canónicos:**

`granjero` · `huerta` · `celda` · `cultivo` · `cosecha` · `herramienta` · `potenciador` · `dispositivo` · `mochila` · `computadora` · `intercambio` · `publicación` · `partida` · `Points (P)` · `PP Coins (PC)` · `XP` · `nivel` · `eficiencia` · `durabilidad` · `rareza`.

> **Decisión pendiente recomendada:** hoy el HUD usa “PP” para los Points (moneda plateada) y “PC” para las PP Coins (moneda dorada), mientras la tienda llama “PP Coins” a las monedas. Eso vuelve ambiguo el símbolo. La regla propuesta es: **P = Points (plata)**, **PC = PP Coins (oro)**. No usar “PP” solo para los Points. Ver [§9](#9-estado-actual-y-deuda-de-diseño).

### 1.6 Momentos que el estilo debe proteger

Estos momentos merecen feedback propio; no deben diluirse en un mensaje genérico:

1. **Primer clic en una celda** — el cursor cambia, el cultivo se sacude, la barra de vida aparece.
2. **Primera cosecha** — popup flotante con `+XP` azul y `+P` dorado (el HUD actual muestra `+PP`), desaparición del cultivo.
3. **Primera subida de nivel** — se desbloquean celdas, cambia la skin del cultivo, se actualiza el HUD.
4. **Primera conversión** — el jugador ve cuántos Points entrega y cuántos PP Coins recibe antes de confirmar.
5. **Primera compra de herramienta** — el saldo baja, el ítem aparece en la mochila, el frame equipado se actualiza.
6. **Primera venta o publicación** — el ítem sale del inventario y aparece en el mercado.
7. **Primera compra de otro jugador** — el vendedor recibe las PP Coins; el comprador ve el ítem en su mochila.
8. **Guardar partida** — el mensaje confirma que el mundo sigue existiendo fuera de la sesión.

---

## 2. Identidad visual

### 2.1 Norte visual: Pixel Terraria / Retro Farmstead

La dirección visual es **pixel art de 16 bits con gramática de objeto físico**. Las referencias no se copian; se traducen:

- **Terraria:** marcos de madera, celdas de tierra, objetos con borde duro, cursor de herramienta.
- **Stardew Valley:** calidez de granja, ritmo cozy, progresión por días y herramientas.
- **Consolas de 16 bits / CRT:** tipografía pixel, HUD fijo, colores de fósforo, biseles.
- **Mercado de pueblo:** publicaciones, vendedor, comprador, trueque cara a cara.
- **Terminal retro:** `PP-FARM-OS`, pestañas, conversión, saldos, mensajes de sistema.

**Palabras clave:** pixel, madera, pasto ajedrezado, tierra, oro, bisel, CRT, feria, terminal, cosecha, trueque.

**Regla rectora:** *Todo lo que se ve debe poder dibujarse con píxeles; todo lo que se toca debe poder golpearse.*

### 2.2 Paleta

La paleta se organiza por **función**, no por gusto. Los tokens viven en `wwwroot/css/pixel.css` y son la fuente de verdad.

#### Mundo y estructura

| Token | Hex | Uso |
|---|---|---|
| `--tf-grass` | `#8bc34a` | Base del pasto |
| `--tf-grass-light` | `#7cb342` | Damero claro |
| `--tf-grass-dark` | `#8cd634` | Damero oscuro |
| `--tf-grass-dark-light` | `#78be28` | Variante de damero |
| — (body) | `#2e5d3b` | Fondo del body y barra lateral (sin token propio) |
| `--tf-panel` | `#382419` | Panel de madera |
| `--tf-panel-deep` | `#140b07` | Borde exterior / sombra estructural |
| `--tf-panel-bevel-hi` | `#5c3c28` | Bisel superior-izquierdo |
| `--tf-panel-bevel-lo` | `#22140c` | Bisel inferior-derecho |
| `--tf-header` | `#23160e` | Cabecera de modal |
| `--tf-header-lo` | `#3a2214` | Línea inferior de cabecera |
| `--tf-content` | `#2c1b12` | Cuerpo de modal |
| `--tf-card` | `#1e130c` | Tarjeta / slot |
| `--tf-card-hi` | `#3a2517` | Bisel de tarjeta |
| `--tf-card-lo` | `#0c0603` | Sombra de tarjeta |
| `--tf-fence` | `#3e2213` | Cerca de la huerta |
| `--tf-fence-hi` | `#5e341c` | Bisel de la cerca |
| `--tf-fence-outline` | `#140904` | Contorno de la cerca |
| `--tf-soil` | `#4a240f` | Tierra de celda |
| `--tf-soil-hi` | `#6d3819` | Bisel de tierra |
| `--tf-soil-lo` | `#1e0d04` | Sombra de tierra |
| `--tf-grid-bg` | `#2a1307` | Fondo de la cuadrícula |
| `--tf-grid-border` | `#190a03` | Borde de la cuadrícula |

#### Valor, acción y estado

| Token | Hex | Uso |
|---|---|---|
| `--tf-gold` | `#ffd84d` | Oro principal: títulos, foco, moneda, equipado |
| `--tf-gold-hi` | `#ffcc00` | Brillo de oro |
| `--tf-gold-lo` | `#8a5a00` | Sombra de oro |
| `--tf-green` | `#258a46` | Acción primaria |
| `--tf-green-hover` | `#2da353` | Hover primario |
| `--tf-green-bevel-hi` | `#5fe088` | Bisel claro del verde |
| `--tf-green-bevel-lo` | `#135227` | Bisel oscuro del verde |
| `--tf-green-border` | `#0d381b` | Borde del verde |
| `--tf-red` | `#b92b27` | Cerrar / peligro |
| `--tf-red-hover` | `#d43531` | Hover de peligro |
| `--tf-slate` | `#2b3945` | Botón secundario / navegación |
| `--tf-slate-hover` | `#3b4b59` | Hover secundario |
| `--tf-slate-border` | `#111827` | Borde secundario |
| `--tf-slate-bevel-hi` | `#475569` | Bisel secundario claro |
| `--tf-slate-bevel-lo` | `#0f172a` | Bisel secundario oscuro |
| `--tf-input-bg` | `#0f0906` | Fondo de input |
| `--tf-input-border` | `#4a2c1b` | Borde de input |
| `--tf-input-focus` | `#ffd84d` | Foco de input |

#### Texto y rarezas

| Token | Hex | Uso |
|---|---|---|
| `--tf-text` | `#f7eed7` | Texto principal sobre madera |
| `--tf-text-dim` | `#cfbda8` | Texto secundario |
| `--tf-text-mute` | `#bfa588` | Metadatos / ayudas |
| `--tf-stat` | `#f6c876` | Estadísticas |
| `--tf-price` | `#ffe66d` | Precio |
| `--tf-ok` | `#55ff99` | Saldo / éxito |
| `--tf-ok-text` | `#7aff9b` | Texto de éxito |
| `--tf-error-bg` | `#4a1512` | Fondo de error |
| `--tf-error-border` | `#2a0a08` | Borde de error |
| `--tf-error-text` | `#ff9d97` | Texto de error |
| `--tf-rarity-comun` | `#a3a3a3` | Rareza común |
| `--tf-rarity-raro` | `#38bdf8` | Rareza rara |
| `--tf-rarity-epico` | `#c084fc` | Rareza épica |
| `#f59e0b` | `#f59e0b` | Rareza legendaria (clase CSS) |

#### Reglas semánticas

1. **Pasto = mundo.** Es fondo, nunca texto ni acción.
2. **Madera = contenedor.** Estructura, no interactividad.
3. **Tierra = objetivo.** Las celdas son lo que se golpea; el hover sube `brightness(1.12)`.
4. **Oro = valor.** Se reserva para números, títulos, foco, moneda y anillo equipado. No usar oro para párrafos largos.
5. **Verde = acción primaria y éxito.** **Rojo = cerrar, peligro y error.** **Pizarra = secundario y navegación.**
6. **Azul = XP.** **Plata = Points.** **Oro = PP Coins.** No mezclar estos tres códigos.
7. **Rareza = borde izquierdo de 5px + etiqueta.** Nunca colorear la tarjeta entera.
8. **Máximo tres acentos por componente.** Si todo brilla, nada brilla.
9. **Contraste:** el oro sobre madera profunda funciona; el texto apagado es solo para metadatos. Verificar WCAG antes de sumar colores nuevos.

### 2.3 Tipografía

Dos familias, sin excepciones:

| Rol | Familia | Tamaños | Uso |
|---|---|---|---|
| **Display / pixel** | `Press Start 2P`, `monospace` | 8–13px en UI; 26–62px en título | Títulos de modal, botones, labels, números, HUD, “PP FARM” |
| **Cuerpo / retro** | `Silkscreen`, `monospace` | 8–10px | Descripciones, estadísticas, mensajes, ayudas, footer |

**Reglas:**

- **Mayúsculas por CSS**, no en el texto fuente: `text-transform: uppercase` para labels, botones y títulos de cabecera.
- **Press Start 2P no se usa para frases completas.** A 8px es legible para “TIENDA”, no para una descripción.
- **Sombra de texto dura**, sin blur: `2px 2px 0 #000` o `1px 1px 0 #000`. Solo sobre fondos oscuros o de color.
- **Letter-spacing** de `0.03em` a `0.08em` en labels pequeños; nunca tracking negativo.
- **Dos pesos máximo por pantalla.** Silkscreen 400 para cuerpo, 700 para énfasis.
- **Tamaño mínimo recomendado:** 10px para texto corrido; 8px solo para metadatos y badges. Si el texto es un párrafo, subir a 10–11px.

**Jerarquía actual:**

| Nivel | Ejemplo | Fuente | Tamaño |
|---|---|---|---|
| Título de juego | `PP FARM` | Press Start 2P | `clamp(26px, 7.5vw, 62px)` |
| Título de modal | `TIENDA DE LA HUERTA` | Press Start 2P | 13px |
| Título de tarjeta | Nombre del recurso | Press Start 2P | 10px |
| Botón | `COMPRAR` | Press Start 2P | 8.5px |
| Label de campo | `CORREO ELECTRÓNICO` | Silkscreen | 8px |
| Cuerpo | Descripción del recurso | Silkscreen | 8px |
| Estadística | `⚡ 15 · 🛡 100` | Silkscreen | 7.5px |
| Footer | `PP-FARM-OS` | Silkscreen | 8px |

### 2.4 Geometría, bordes y profundidad

**Regla absoluta: radio 0.** Botones, modales, inputs, tarjetas, slots y badges tienen esquinas de 90°. Las excepciones actuales (`border-radius: 2px` en estadísticas) deben eliminarse en la próxima pasada.

La profundidad se construye con **capas de borde y sombras duras**, no con blur:

```css
/* Panel estándar de madera */
.panel {
  background: var(--tf-panel);
  border: 2px solid var(--tf-panel-deep);
  box-shadow:
    inset 3px 3px 0 var(--tf-panel-bevel-hi),
    inset -3px -3px 0 var(--tf-panel-bevel-lo),
    0 4px 0 #000;
}

/* Botón primario */
.boton-primario {
  background: var(--tf-green);
  border: 2px solid var(--tf-green-border);
  box-shadow:
    inset 2px 2px 0 var(--tf-green-bevel-hi),
    inset -2px -2px 0 var(--tf-green-bevel-lo),
    1px 1px 0 #000;
}

/* Slot / input hundido */
.slot {
  background: var(--tf-input-bg);
  border: 2px solid var(--tf-input-border);
  box-shadow: inset 2px 2px 0 #000, inset -1px -1px 0 var(--tf-card-hi);
}
```

**Niveles de elevación:**

| Nivel | Uso | Construcción |
|---|---|---|
| **E0 — Mundo** | Pasto, fondo | Sin sombra; damero animado |
| **E1 — Tarjeta / slot** | Recursos, celdas | Borde 3px + bisel interior |
| **E2 — Botón** | Acciones | Bisel interior + `0 3px 0 #000` |
| **E3 — Modal** | Tienda, mochila | Borde 4px + bisel interior + `0 8px 0 rgba(0,0,0,.8)` + contorno 2px |
| **E4 — Overlay** | Fondo de modal | `rgba(0,0,0,.65)`, sin blur |

**Estados físicos:**

- **Hover:** subir 1px, aclarar fondo, aumentar sombra inferior.
- **Active:** bajar 2px, invertir bisel, reducir sombra a 1px.
- **Disabled:** opacidad 0.45–0.6, sin hover, cursor `not-allowed`.
- **Focus:** borde dorado de 2px, sin `outline` difuso. El foco debe verse incluso sobre madera.

**Acentos de esquina:** cuadrados de 12×12px en oro, con borde `--tf-panel-deep`, sobresaliendo 4px del frame. Son un detalle de identidad del login/registro. No usarlos en todos los paneles; pierden valor si se repiten.

### 2.5 Grilla y espaciado

La base es **4px**, con ritmo principal de **8px**.

| Token / medida | Valor | Uso |
|---|---|---|
| `space-xs` | 4px | Separación mínima |
| `space-sm` | 8px | Gap entre slots, padding de tarjeta |
| `space-md` | 16px | Separación de bloques |
| `space-lg` | 24px | Padding de sección |
| `space-xl` | 32px | Separación de pantallas |
| `margin` | 24px | Margen de canvas |
| `gutter` | 16px | Columna / grilla |

**Medidas clave:**

- Pantalla de juego: `100dvh`, `overflow: hidden`; el scroll vive dentro de modales.
- Barra lateral: `clamp(180px, 17vw, 260px)`.
- Contenedor de juego: `padding: clamp(10px, 1.5vw, 20px)`.
- Huerta: máximo 5 columnas y 3 filas; celdas cuadradas con `aspect-ratio: 1 / 1`.
- Celda máxima: `clamp(46px, min(11vw, (100dvh - 240px) / 3), 230px)`.
- Gap de huerta: `clamp(5px, 0.6vw, 14px)`.
- Modal: `max-width: 680px`, `max-height: 94vh`, contenido con `max-height: 380px` y scroll interno.
- Auth: frame `max-width: 560px`, padding 4px, panel con borde 2px.
- Frame equipado: 76×76px, fijo a 20px del borde inferior derecho.
- Slot de inventario de referencia: 44–56px; slot de hotbar: 44px.

**Breakpoints:**

| Breakpoint | Cambio |
|---|---|
| `≤ 700px` | Home pasa de fila a columna; sidebar se vuelve barra superior; botones en wrap; modal a pantalla completa menos margen |
| `≤ 480px` | Padding de auth reducido; tipografía de título con `clamp` |
| `≤ 420px` | Botones de acción ocupan todo el ancho |
| `max-height: 700px` | La huerta se pega al header en vertical |

### 2.6 Imágenes, sprites e iconos

- `image-rendering: pixelated` en todas las imágenes.
- Escalado **entero** siempre que sea posible (×1, ×2, ×3). Evitar tamaños arbitrarios que rompan el píxel.
- No aplicar filtros suaves ni sombras difusas a los sprites. La única excepción es el flash de golpe (`brightness`/`sepia`).
- `user-select: none` y `-webkit-user-drag: none` en imágenes de juego.

**Assets actuales:**

| Ruta | Contenido | Estado |
|---|---|---|
| `/images/cultivos/Cultivo1.png` … `Cultivo3.png` | Skins de cultivo | Solo 1–3 implementadas |
| `/images/tools/default/Main.png`, `Click.png` | Herramienta por defecto | OK |
| `/images/tools/pala/Main.png`, `Click.png` | Pala | OK |
| `/images/tools/pico/Main.png`, `Click.png` | Pico | OK |
| `/images/tools/pala-de-oro/Main.png`, `Click.png` | Pala de oro | OK |
| `/images/ui/equipped-frame/EquippedItemFrame.png` | Frame equipado | Legado; el estilo actual usa panel CSS |

> **Deuda de contenido:** `CultivosSkin` mapea las skins 1–9, pero solo existen `Cultivo1–3.png`. A partir del nivel 20 pueden aparecer rutas 404. Ver [§9](#9-estado-actual-y-deuda-de-diseño).

**Convención de nombres:**

- Cultivos: `Cultivo{n}.png`.
- Herramientas: `/images/tools/{nombre-kebab}/Main.png` y `Click.png`.
- Nombres en minúsculas, sin espacios, con guiones.
- Si se agrega una herramienta nueva, debe existir al menos `Main.png`; `Click.png` es el estado presionado.

**Cursores:**

- `Main.png` en reposo; `Click.png` al presionar.
- Hotspot coherente (punta de la herramienta).
- Fallback `auto` si la imagen no carga.
- Solo en dispositivos con puntero fino; en touch no aplicar cursor custom.

**Política de emojis:**

Hoy se usan como iconos provisorios: `🪏`, `⛏️`, `🌾`, `💧`, `🍅`, `🥕`, `🌽`, `🌱`, `📦`, `💰`, `🎒`, `⚠`, etc. La dirección final es **reemplazarlos por sprites pixel art**. Mientras se usen:

- Máximo un emoji por etiqueta.
- No usar emoji como única señal de estado (agregar texto).
- Evitar emojis que cambian de forma entre sistemas operativos.
- No mezclar emoji con iconos vectoriales en el mismo bloque.

### 2.7 Movimiento y feedback

| Efecto | Duración | Easing | Disparador | Notas |
|---|---|---|---|---|
| Pasto animado | 2.6s | `linear` infinito | Siempre en Home/Login/Register | Desplaza exactamente un tile de 48px |
| Título flotante | 3.2s | `ease-in-out` infinito | Login/Register | `translateY(-11px)` + glow |
| Crecimiento de cultivo | 0.8s | `ease-out` | Al quedar listo | Escala desde 0.3 |
| Golpe de cultivo | 0.3s | `cubic-bezier(.36,.07,.19,.97)` | Clic | Sacudida + flash |
| Desaparición de cultivo | 1s | `ease-in-out` | Cosecha | Rotación decreciente + fade |
| Popup de cosecha | 2s | `ease-out` | Cosecha | Sube 52px y se desvanece |
| Hover de botón | 0.05s | `steps(2)` | Puntero | Se siente “digital”, no fluido |
| Active de botón | 0.05s | `steps(2)` | Clic | Baja 2px |
| Aparición de modal | Opcional 120ms | `steps(3)` | Abrir | No implementado aún |

**Reglas:**

- Animar **transform, opacity y filter**. No animar `width`, `height`, `top` o `left` salvo que sea inevitable.
- UI con `steps()` para conservar la sensación de píxel; mundo con `ease-out` para conservar la sensación física.
- No usar blur en sombras.
- El pasto y el título ignoran `prefers-reduced-motion` **a propósito** en el código actual, porque el movimiento es parte del diseño. Aun así, debe existir una opción “Reducir movimiento” en Configuración para quien lo necesite. Ver [§7](#7-accesibilidad-y-ergonomía).

---

## 3. Arquitectura de la interfaz

### 3.1 Shell de juego (Home)

```text
┌──────────────────────────────────────────────────────────────┐
│  SIDEBAR (verde)          │  GAME AREA (pasto ajedrezado)    │
│                           │                                  │
│  Granjerotest             │  Página principal     PPFarmWA   │
│  NIVEL 12                 │                                  │
│  XP 348 / 588             │      ╔══════ CERCA ══════╗       │
│  [P] 12800   [PC] 3450    │      ║ [🌱] [🌱] [🌱]    ║       │
│                           │      ║ [🌱] [🌱] [🌱]    ║       │
│  [ TIENDA ]               │      ║ [🌱] [🌱] [🌱]    ║       │
│  [ MOCHILA ]              │      ╚═══════════════════╝       │
│  [ COMPUTADORA ]          │                                  │
│  [ INTERCAMBIOS ]         │                        [🔨]      │
│  [ CONFIGURACIÓN ]        │                     equipado     │
│  [ GUARDAR PARTIDA ]      │                                  │
│  “Partida sincronizada”   │                                  │
└──────────────────────────────────────────────────────────────┘
```

**Anatomía:**

| Zona | Contenido | Regla |
|---|---|---|
| **Barra lateral** | Identidad, nivel, XP, monedas, acciones, guardado | Fondo verde `#2e5d3b`, borde derecho 4px, texto Press Start 2P |
| **Identidad** | Nombre del jugador | 12px, oro `#fde047`, borde inferior |
| **Progreso** | Nivel y XP | Filas separadas; XP con divisoria punteada |
| **Monedas** | Points y PP Coins | Chip de 24×24 + valor 11px |
| **Acciones** | Tienda, mochila, computadora, intercambios, configuración | Botones pizarra de ancho completo |
| **Guardado** | Guardar partida + mensaje temporal | Verde; el mensaje aparece 3s |
| **Cabecera de juego** | Nombre de pantalla + versión | Texto oscuro sobre pasto, sombra clara |
| **Huerta** | Cerca + grid de celdas | Centrada; crece con el viewport |
| **Frame equipado** | Herramienta activa | Fijo abajo a la derecha; anillo dorado |

**Capas z-index actuales:**

| Capa | z-index |
|---|---|
| Fondo de pasto | 0 |
| Huerta / celdas | 1–10 |
| Barra de vida | 10 |
| Popup de cosecha | 40 |
| Frame equipado | 100 |
| Modal | 2000 |

### 3.2 Shell de autenticación (Login / Register)

```text
        PASTO ANIMADO A PANTALLA COMPLETA

                    PP FARM

              ┌─ oro ─────────── oro ─┐
              │  ■  INICIAR SESIÓN  ■  │
              │                        │
              │  CORREO ELECTRÓNICO    │
              │  [__________________]  │
              │  CONTRASEÑA            │
              │  [__________________]  │
              │                        │
              │  [ INICIAR SESIÓN ]    │
              │  [ CREAR CUENTA ]      │
              │                        │
              │  PP-FARM-OS            │
              └─ oro ─────────── oro ─┘
```

- Fondo: `.terrain-bg` a pantalla completa.
- Título: `.pf-title`, Press Start 2P, oro, flotante.
- Frame: `.pf-frame`, `max-width: 560px`, fondo `#0d1017`, padding 4px, sombra dura, contorno negro.
- Panel: `.pf-panel`, madera, borde 2px, bisel interior.
- Cabecera: título centrado + “pips” decorativos.
- Cuerpo: campos, error, acciones.
- Footer: `PP-FARM-OS` a la derecha; estado del servidor a la izquierda (referencia).
- Acentos de esquina: `.pf-corner`, 12×12px oro.

**Login:** correo + contraseña + “Iniciar sesión” + “Crear cuenta”.
**Register:** usuario + correo + contraseña + “Crear cuenta” + “Volver al login”.

> **Decisión de identidad:** la referencia Stitch original tenía un shell navy de “terminal” para auth; la implementación actual convergió a madera Terraria. Este documento adopta **madera** como canónico para no partir la experiencia. Si se quiere recuperar el navy, debe ser un cambio deliberado de subsistema (por ejemplo, “modo terminal”), no una inconsistencia accidental.

### 3.3 Shell de modal (Tienda / Mochila)

```text
        FONDO DEL JUEGO OSCURECIDO (rgba 0,0,0,.65)

        ┌──────────── MODAL DE MADERA ────────────┐
        │  🌱 TIENDA DE LA HUERTA          [×]   │
        │  Conseguí recursos para tu huerta.      │
        ├─────────────────────────────────────────┤
        │  💰 PP Coins: 150.00                    │
        ├─────────────────────────────────────────┤
        │  [mensaje de éxito / error]             │
        ├─────────────────────────────────────────┤
        │  ┌ tarjeta ──────────────────────────┐  │
        │  │ [icono] Nombre                    │  │
        │  │         descripción               │  │
        │  │         ⚡ · 🛡 · 🔨 · 💎          │  │
        │  │         💰 precio                 │  │
        │  │                  [cantidad] [BTN] │  │
        │  └───────────────────────────────────┘  │
        │  … scroll interno …                     │
        └─────────────────────────────────────────┘
```

- Overlay: `.modal-fondo`, fijo, `rgba(0,0,0,.65)`, sin blur.
- Panel: `.modal-juego`, `max-width: 680px`, `max-height: 94vh`, madera, borde 4px, sombra dura.
- Cabecera: `.modal-cabecera`, fondo `#23160e`, título oro, subtítulo muted.
- Cerrar: `.boton-cerrar`, 28×28px, rojo, borde oscuro, bisel.
- Saldo: `.saldo`, fondo `#180e09`, borde inferior, texto dorado.
- Mensajes: `.mensaje-exito` verde / `.mensaje-error` rojo.
- Contenido: `.modal-contenido`, scroll interno, scrollbar pixelada de 8px.
- Tarjetas: `.tarjeta-recurso`, borde 3px, bisel, rareza como borde izquierdo de 5px.
- En móvil: el modal ocupa el ancho completo menos 12px; contenido apilado.

### 3.4 Shell de computadora / terminal (objetivo)

La pantalla `/internet` representa la computadora de la granja. Hoy usa una estética web clara genérica; la dirección debe ser **una terminal dentro del mundo**:

- Ventana con marco oscuro/pizarra, no una tarjeta blanca.
- Pestañas `Convertir` / `Intercambiar` como botones pixel.
- Fondo de fósforo oscuro (slate) con texto verde/ámbar; opcional scanlines muy sutiles.
- Saldos siempre visibles: `Points` y `PP Coins`.
- Formulario de conversión con tasa visible (“10 Points = 1 PP Coin”).
- Historial de conversiones como líneas de terminal.
- Acción de mercado que abre `/intercambios` o consume el mismo endpoint, sin duplicar lógica.
- Footer `PP-FARM-OS`.

> Esta pantalla es la deuda visual más grande del producto. Ver [§9](#9-estado-actual-y-deuda-de-diseño).

### 3.5 Responsive

| Viewport | Home | Auth | Modales |
|---|---|---|---|
| **1440×900 / 1920×1080** | Sidebar 180–260px; huerta centrada con 5 columnas; frame equipado abajo a la derecha | Frame 560px centrado | 680px centrado |
| **768×1024** | Sidebar se angosta; huerta se adapta; celdas más chicas | Frame 560px con márgenes | 680px o full-width |
| **390×844** | Sidebar arriba en wrap; huerta vertical; botones full-width; frame equipado puede tapar celdas | Frame full-width menos 4px; título más chico | Full-screen menos 1rem; tarjetas apiladas |

**Reglas:**

- Nunca scroll horizontal.
- En móvil, el frame equipado no debe tapar la última fila de cultivos: moverlo al header o reducirlo.
- Los botones deben tener un área táctil mínima de 44×44px, aunque el dibujo visible sea más chico.
- Los modales deben poder cerrarse con `Esc`, con clic en overlay y con el botón `×`.

---

## 4. Componentes

### 4.1 Botones

| Clase | Fondo | Borde | Texto | Uso |
|---|---|---|---|---|
| `.custom-btn` | `--tf-slate` | 3px `--tf-slate-border` | `#f8fafc` | Navegación de sidebar |
| `.custom-btn:hover` | `--tf-slate-hover` | — | — | Sube 1px |
| `.custom-btn:active` | — | — | — | Baja 2px, bisel invertido |
| `.btn-guardar` | `#2e7d32` | 3px | Blanco | Guardar partida |
| `.boton-primario` | `--tf-green` | 2px `--tf-green-border` | Blanco | Comprar, vender, confirmar |
| `.boton-secundario` | `#33384f` | 2px `#1b1e2c` | `#e2e8f0` | Acción secundaria |
| `.boton-equipar` | `#a37a2e` | 2px `#6b4c1a` | Blanco | Equipar herramienta |
| `.boton-equipar.equipado` | `--tf-green` | `--tf-green-border` | Blanco | Estado equipado |
| `.boton-cerrar` | `--tf-red` | 2px `#520e0c` | Blanco | Cerrar modal |
| `.btn-volver` | *Pendiente* | *Pendiente* | *Pendiente* | Volver; hoy genérico |

**Patrón de botón primario:**

```css
.boton-primario {
  padding: 7px 10px;
  border: 2px solid var(--tf-green-border);
  border-radius: 0;
  background: var(--tf-green);
  box-shadow:
    inset 2px 2px 0 var(--tf-green-bevel-hi),
    inset -2px -2px 0 var(--tf-green-bevel-lo),
    1px 1px 0 #000;
  color: #fff;
  font-family: var(--tf-font-pixel);
  font-size: 8.5px;
  letter-spacing: .5px;
  text-transform: uppercase;
  text-shadow: 1px 1px 0 #000;
}
.boton-primario:hover { background: var(--tf-green-hover); }
.boton-primario:active {
  transform: translateY(1px);
  box-shadow:
    inset 2px 2px 0 var(--tf-green-bevel-lo),
    inset -2px -2px 0 var(--tf-green-bevel-hi);
}
```

**Reglas:**

- Un solo botón primario por bloque de decisión.
- “Comprar” y “Confirmar” son verdes; “Cancelar”/“Volver” son pizarra; “Cerrar”/“Eliminar” son rojos.
- Nunca usar rojo para una acción positiva.
- El texto de un botón es un verbo: `COMPRAR`, `VENDER`, `EQUIPAR`, `PUBLICAR`, `CONFIRMAR`, `GUARDAR PARTIDA`.
- Estado ocupado: cambiar el label a `PROCESANDO…` y deshabilitar. No usar spinner genérico.

### 4.2 Inputs y formularios

**Auth (`.pf-input`):**

- Fondo `--tf-input-bg`, borde 2px `--tf-input-border`, sin radio.
- Texto blanco, Press Start 2P 8px, sombra 1px.
- Placeholder gris pizarra, sin sombra.
- Foco: borde `--tf-input-focus` (oro), sin glow.
- Label arriba, Silkscreen 8px, uppercase, muted, con icono opcional.
- Error: caja `.pf-error` roja con borde e inset; texto 8px.

**Inputs numéricos (`.acciones-tienda input`):**

- Ancho 44px, padding 4px 2px, centrado.
- Fondo `#0f0906`, borde 2px `#4a2c1b`, sin radio.
- Foco: borde oro.
- `min`/`max` visibles en el copy o en el propio input; validar en UI y servidor.

**Selects (mercado):**

- Misma familia y borde que un input.
- Evitar el look nativo del navegador si rompe la estética; usar fondo oscuro y flecha pixel.
- En móvil, el select nativo es aceptable por usabilidad, pero debe conservar colores y tipografía.

**Reglas:**

- Un campo = una intención. No pedir datos que el sistema ya sabe.
- Mostrar el error junto al campo, no solo en un toast.
- No usar placeholders como labels.
- `autocomplete` correcto en login/registro.
- `inputmode="numeric"` en campos numéricos móviles.

### 4.3 Tarjetas de recurso / inventario

```text
┌─ borde de rareza (5px) ──────────────────────────────┐
│  ┌────────┐  NOMBRE DEL RECURSO                     │
│  │ icono  │  Descripción breve del recurso.          │
│  │ 44×44  │  ⚡ 15 · 🛡 100 · 🔨 Herramienta · 💎 Raro│
│  └────────┘  💰 25.00 PP Coins c/u                   │
│                                       [cantidad] [COMPRAR] │
└──────────────────────────────────────────────────────┘
```

- Fondo `--tf-card`, borde 3px `#100905`, bisel interior.
- Rareza: borde izquierdo de 5px (`común` gris, `raro` azul, `épico` violeta, `legendario` ámbar).
- Icono: 44×44px, fondo `#121824`, borde 3px, bisel.
- Nombre: Press Start 2P 10px, blanco, sombra.
- Descripción: Silkscreen 8px, `--tf-text-dim`.
- Estadísticas: bloque con fondo `#120b06`, borde 1px, texto `--tf-stat` 7.5px.
- Precio: Press Start 2P 8.5px, `--tf-price`.
- Acciones: a la derecha en desktop; abajo a la derecha en móvil.
- No mostrar la descripción completa si es un párrafo largo: truncar a 2 líneas o usar tooltip.

### 4.4 Chips de moneda

| Chip | Color | Símbolo | Significado |
|---|---|---|---|
| Points | Plata (`#e2e8f0` → `#8a99ad`) | `P` | Progreso de cosecha, no monetario |
| PP Coins | Oro (`#ffe600` → `#d49800`) | `PC` | Moneda gastable / convertible |

- Tamaño 24×24px, borde 2px, bisel interior, sin radio.
- Valor al lado: 11px, bold, blanco, sombra 1px.
- En la tienda, el saldo se muestra como `💰 PP Coins: 150.00`.
- **Nunca** usar el mismo símbolo para Points y PP Coins.
- En listas, alinear los chips a la izquierda y los valores a la derecha.

### 4.5 Celda de cultivo y barra de vida

- Celda: cuadrado perfecto, fondo `--tf-soil`, borde 2px `#271206`, bisel interior `--tf-soil-hi` / `--tf-soil-lo`.
- Hover: `filter: brightness(1.12)`.
- Cultivo: `width/height 100%`, `object-fit: contain`, `transform-origin: bottom center`.
- Barra de vida: 4px de alto, `top: 3px`, `left/right: 5px`, fondo `#1a1a1a`, borde 1px negro.
- Color según porcentaje: `>50%` verde `#22c55e`, `>20%` amarillo `#facc15`, `≤20%` rojo `#ef4444`.
- La barra solo aparece cuando el cultivo fue golpeado y todavía no fue cosechado.
- Estados del cultivo:
  - `listo`: crece con animación de 0.8s.
  - `hit`: se sacude 0.3s.
  - `cosechado`: se desvanece 1s.
  - `standby`: espera el cooldown de golpe (350ms).

### 4.6 Popup de cosecha

- Aparece sobre la celda, `bottom: 100%`, centrado.
- Fondo `rgba(20, 11, 7, .85)`, borde 2px oro.
- Dos textos: `+X XP` en azul `#38bdf8` y `+Y P` en dorado `#facc15`. Hoy el popup muestra `+Y PP`; debe migrar al símbolo canónico `P` (ver §9).
- Fuente Press Start 2P 10px, bold.
- Animación `harvestRise` de 2s: aparece, escala, sube 52px y se desvanece.
- No bloquear el clic; `pointer-events: none`.
- No acumular popups: si se cosecha rápido, reiniciar la animación de la celda.

### 4.7 Frame equipado y hotbar

**Implementado:**

- `.equipped-frame`: 76×76px, fijo a 20px de la derecha e inferior, fondo `--tf-card`, borde 3px, bisel, anillo dorado interior.
- Imagen `object-fit: contain`, pixelada.
- `pointer-events: none`; es información, no botón.
- En móvil, reducir a 56–64px o moverlo al header para no tapar cultivos.

**Objetivo (referencia de diseño, aún no implementado):**

- Hotbar de 5 slots de 44×44px, con número `1–5`, icono y cantidad.
- Slot activo con borde oro y glow suave (excepción permitida al “no blur”, solo para el slot activo).
- El frame equipado puede convivir con la hotbar en la parte inferior.
- Teclas `1–5` para cambiar herramienta; `Tab` o `E` para abrir mochila.

### 4.8 Modales

**Estructura:**

```html
<div class="modal-fondo" @onclick="Cerrar">
  <section class="modal-juego" role="dialog" aria-modal="true" aria-label="Tienda"
           @onclick:stopPropagation="true">
    <header class="modal-cabecera">
      <div><h1>🌱 Tienda de la Huerta</h1><p>Conseguí recursos para tu huerta.</p></div>
      <button class="boton-cerrar" aria-label="Cerrar tienda">×</button>
    </header>
    <div class="saldo">💰 PP Coins: <strong>150.00</strong></div>
    <p class="mensaje-exito">¡Compra exitosa!</p>
    <div class="modal-contenido">…tarjetas…</div>
  </section>
</div>
```

**Reglas:**

- Overlay cierra al hacer clic; el panel detiene la propagación.
- `role="dialog"`, `aria-modal="true"`, `aria-label` obligatorio.
- `Esc` cierra.
- Foco inicial en el primer campo o en el botón de cerrar; foco atrapado dentro del modal.
- El scroll es interno; el body no scrollea.
- Scrollbar: 8px, track `#170d08`, thumb `#5c3c28`, borde 2px.
- Un modal no abre otro modal encima; si hace falta confirmar, usar un panel de confirmación dentro del mismo modal.

### 4.9 Panel de autenticación

- `.pf-frame`: `max-width: 560px`, fondo `#0d1017`, padding 4px, sombra `0 8px 0 rgba(0,0,0,.8), 0 0 0 2px #000`.
- `.pf-panel`: madera, borde 2px, bisel interior 3px.
- `.pf-cabecera`: fondo `--tf-header`, borde inferior 4px, pips de 8px/6px.
- `.pf-body`: gap 12px, padding 14px.
- `.pf-footer`: fondo `--tf-header`, borde superior 2px, texto 8px.
- Título de página: `PP FARM`, Press Start 2P, oro, flotante, sombra de 4 capas.
- El formulario no debe tener fondo propio; el panel es el fondo.
- El botón “Crear cuenta” usa `.custom-btn` (pizarra), no compite con el primario.

### 4.10 Mensajes y estados

| Estado | Componente | Color | Copy |
|---|---|---|---|
| **Cargando** | `.estado-modal` / `.pf-loading` | Muted / oro | “Cargando recursos...”, “Guardando granjero...” |
| **Vacío** | `.estado-modal` | Muted | “No hay recursos disponibles.”, “Tu inventario está vacío.” |
| **Error** | `.mensaje-error` / `.pf-error` | Rojo | “No se pudo completar la operación.” + causa |
| **Éxito** | `.mensaje-exito` | Verde | “¡Compra exitosa! Se equipó en tu mochila.” |
| **Ocupado** | Botón disabled | Opacidad 0.55 | “Procesando...”, “Guardando...” |
| **Sin saldo** | Botón disabled | Opacidad 0.55 | El precio se mantiene visible; explicar por qué está deshabilitado |

**Reglas:**

- Un mensaje aparece **cerca de la acción** que lo produjo.
- Los mensajes de éxito se autodestruyen a los 3s; los errores permanecen hasta que el usuario actúe.
- No usar `alert()` ni `confirm()` del navegador.
- No mostrar excepciones crudas ni stack traces.
- El estado vacío debe decir qué hacer a continuación, no solo que no hay nada.

### 4.11 Scrollbar pixelada

```css
.modal-contenido::-webkit-scrollbar { width: 8px; }
.modal-contenido::-webkit-scrollbar-track { background: #170d08; }
.modal-contenido::-webkit-scrollbar-thumb {
  background: #5c3c28;
  border: 2px solid #170d08;
}
.modal-contenido::-webkit-scrollbar-thumb:hover { background: #7a5036; }
```

- La scrollbar es parte del marco, no un detalle del navegador.
- En Firefox, usar `scrollbar-color` equivalente.
- No ocultar la scrollbar si hay contenido scrolleable.

---

## 5. Pantallas y flujos

### 5.1 Mapa de rutas

| Ruta | Pantalla | Shell | Estado |
|---|---|---|---|
| `/login` | Iniciar sesión | Auth (pasto + madera) | Implementada |
| `/register` | Crear cuenta | Auth (pasto + madera) | Implementada |
| `/` | Home / huerta | Juego (sidebar + pasto) | Implementada |
| `/tienda` | Tienda como página | Modal | Wrapper de `TiendaModal` |
| `/inventario` | Mochila como página | Modal | Wrapper de `InventarioModal` |
| `/internet` | Computadora | Terminal (objetivo) | Implementada con estética web genérica |
| `/intercambios` | Mercado | Página de juego (objetivo) | Implementada con estética web genérica |
| `/not-found` | No encontrado | Ninguno | Implementada, en inglés |

### 5.2 Login

**Propósito:** que el granjero entre a su partida.

**Elementos:** título `PP FARM`, frame de madera, cabecera “Iniciar sesión”, campo correo, campo contraseña, botón primario, botón “Crear cuenta”, footer `PP-FARM-OS`.

**Estados:**

- Inicial: campos vacíos, sin error.
- Cargando: botón `CARGANDO...`, deshabilitado.
- Error de credenciales: caja roja “Correo electrónico o contraseña incorrectos.”
- Error de conexión: mensaje accionable, sin stack trace.
- Éxito: guardar sesión y navegar a `/`.

**Copy:** `CORREO ELECTRÓNICO`, `CONTRASEÑA`, `INICIAR SESIÓN`, `CREAR CUENTA`.

**Deuda:** el login actual muestra el mensaje de error con `ex.Message` en fallos de conexión; debe envolverse en un mensaje amigable.

### 5.3 Register

**Propósito:** crear un granjero nuevo.

**Elementos:** usuario, correo, contraseña (mínimo 8), botón primario “Crear cuenta”, botón secundario “Volver al login”.

**Estados:** cargando “Guardando granjero...”, error de duplicado, error de validación, éxito → Home.

**Reglas:**

- Validar en cliente y servidor.
- No mostrar la contraseña.
- No guardar la contraseña en el estado global.
- Mensaje de duplicado: “El email o el nombre de usuario ya están en uso.”

**Deuda:** el registro actual lee el cuerpo de error de la API y lo muestra tal cual; puede contener texto técnico. Normalizar.

### 5.4 Home

**Propósito:** el núcleo del juego. Cultivar, cosechar, ver progreso, abrir sistemas.

**Jerarquía visual:**

1. Nombre del jugador.
2. Nivel y XP.
3. Monedas.
4. Acciones.
5. Guardar partida.
6. Huerta.
7. Herramienta equipada.

**Estados:**

- Sin jugador: redirigir a `/login`.
- Cargando catálogo: la huerta puede mostrarse, pero las herramientas aún no.
- Nivel nuevo: actualizar celdas y skins; feedback visible.
- Guardado: mensaje temporal verde/rojo.
- Error de guardado: mensaje rojo, mantener al jugador en la pantalla.

**Reglas:**

- El HUD no se tapa con modales.
- La huerta ocupa el centro y es el objeto más grande.
- El frame equipado no debe tapar cultivos.
- El clic en una celda debe sentirse inmediato; el cooldown de 350ms se percibe como ritmo, no como lag.

### 5.5 Tienda

**Propósito:** gastar PP Coins en herramientas, potenciadores y dispositivos.

**Elementos:** cabecera, saldo, mensaje, lista de recursos, tarjeta con icono, nombre, descripción, estadísticas, precio, cantidad y comprar.

**Estados:**

- Cargando: “Cargando recursos...”.
- Vacío: “No hay recursos disponibles.”
- Sin saldo: botón deshabilitado; el precio sigue visible.
- Compra exitosa: mensaje verde + saldo actualizado + inventario actualizado.
- Error: mensaje rojo; no crear ítem.

**Reglas:**

- La cantidad se valida en UI y servidor. Hoy la UI permite 1–99; el backlog hablaba de 1–3. Definir un rango canónico y usarlo en ambos lados.
- El precio total (`valor × cantidad`) debe verse antes de comprar.
- No permitir comprar si el saldo es insuficiente.
- El botón “Comprar” es el único primario de la tarjeta.

### 5.6 Mochila / Inventario

**Propósito:** ver, equipar y vender ítems.

**Elementos:** cabecera, saldo, mensaje, lista de ítems, icono, nombre, descripción, estadísticas, cantidad, precio de venta, vender, equipar.

**Estados:**

- Cargando: “Cargando inventario...”.
- Vacío: “Tu inventario está vacío.”
- Vendiendo: botón ocupado.
- Equipado: botón verde “EQUIPADO” / deshabilitado.
- Error: mensaje rojo.

**Reglas:**

- “Equipar” debe persistir; hoy es estado en memoria (`CambioUltimaHerramienta`). Si se recarga, debe seguir equipado.
- “Vender” debe confirmar el precio total y actualizar saldo/stock.
- El botón de equipar no debe decir `??`; usar un icono pixel o ningún icono.
- Mostrar durabilidad actual / máxima en ítems que la tengan.
- No permitir vender más cantidad que la disponible.

### 5.7 Computadora / Internet

**Propósito:** convertir Points en PP Coins y acceder al mercado.

**Elementos:** pestañas `Convertir` / `Intercambiar`, saldos, formulario de conversión, equivalente calculado, botón “Confirmar conversión”, historial, botón “Abrir mercado”.

**Estados:**

- Sin jugador: redirigir a `/login`.
- Conversión inválida: “Necesitás al menos 10 Points para convertir.”
- Points insuficientes: “No tenés suficientes Points.”
- Éxito: mensaje, saldos actualizados, historial.
- Error de conexión: mensaje amigable.

**Reglas:**

- Mostrar la tasa antes de confirmar: `10 Points = 1 PP Coin`.
- El equivalente se calcula en vivo.
- La conversión es irreversible: confirmar explícitamente.
- No duplicar la lógica del mercado: la pestaña “Intercambiar” debe abrir `/intercambios` o consumir el mismo endpoint.
- La pantalla debe sentirse como una terminal, no como una página blanca.

**Deuda crítica:** `Internet.razor` tiene bloques de código con la etiqueta `razor` al inicio y al final que se renderizan como texto; usa estilos inline y una paleta clara ajena al juego. Ver [§9](#9-estado-actual-y-deuda-de-diseño).

### 5.8 Intercambios / Mercado

**Propósito:** publicar ítems y comprar publicaciones de otros jugadores.

**Elementos:** botón volver, título, formulario de publicación (recurso, cantidad, precio), lista de publicaciones activas, tarjeta con vendedor, recurso, cantidad, precio, botón comprar, modal de confirmación.

**Estados:**

- Cargando: “Cargando intercambios...”.
- Vacío: “No hay intercambios disponibles.”
- Error: “Ocurrió un error al cargar los intercambios.”
- Publicado: mensaje + lista actualizada.
- Compra: confirmación + actualización de saldo/inventario.
- Conflicto: si otro jugador compró primero, mensaje claro y refresco de la lista.

**Reglas:**

- No mostrar las publicaciones propias en la lista de disponibles.
- El comprador debe ver el precio total y el vendedor antes de confirmar.
- Si no hay saldo, deshabilitar la compra.
- La confirmación es un modal dentro del shell de juego, no un `confirm()` nativo.
- La compra debe ser transaccional en servidor; la UI debe manejar el 409 con dignidad.

**Deuda:** la página actual usa tarjetas blancas, bordes redondeados, tipografía de sistema y CSS duplicado. Debe migrarse al shell de juego.

### 5.9 NotFound

**Propósito:** decir que la ruta no existe sin romper la inmersión.

**Copy actual:** “Not Found / Sorry, the content you are looking for does not exist.”

**Dirección:** traducir al español y usar el shell de juego o auth:

> **“Esta parcela no existe.”**
> “Volvé a la huerta y seguí cultivando.”
> `[ VOLVER AL HOME ]`

### 5.10 Flujos principales

#### Onboarding

```text
Registro → Home → primer clic en celda → primera cosecha
        → subida de nivel → desbloqueo de celdas
```

#### Loop central

```text
Golpear cultivo → barra de vida baja → cosecha
→ +XP +Points → subir de nivel → más celdas / nueva skin
→ repetir
```

#### Loop económico

```text
Points → Computadora → Convertir → PP Coins
→ Tienda → comprar herramienta → más eficiencia
→ cosechar más rápido → más Points
```

#### Loop social

```text
Mochila → publicar en mercado → otro jugador compra
→ recibir PP Coins → comprar/publicar de nuevo
```

**Regla de oro:** cada loop debe poder explicarse en una frase y debe tener un momento visual propio.

---

## 6. Economía y progresión

### 6.1 Monedas y recursos

| Recurso | Símbolo | Se obtiene | Se gasta | Rol de diseño |
|---|---|---|---|---|
| **XP** | `XP` (azul) | Cosechar | Subir de nivel | Progreso personal |
| **Points** | `P` (plata) | Cosechar | Convertir a PP Coins | Trabajo acumulado |
| **PP Coins** | `PC` (oro) | Convertir, vender | Comprar, mercado | Economía jugable |
| **Eficiencia** | `⚡` | Herramientas | — | Velocidad de golpe |
| **Durabilidad** | `🛡` | Herramientas | — | Vida útil del ítem |
| **Rareza** | `💎` | Ítems | — | Valor percibido |

### 6.2 Fórmulas actuales

> Estos valores son **tuning**, no constantes sagradas. Si se cambian, actualizar este documento y la UI que los comunica.

| Sistema | Fórmula / valor | Fuente |
|---|---|---|
| Vida del cultivo | `round(10 + (nivel - 1)² × 5)` | `Cultivo.cs` |
| XP por cosecha | `nivel × 5` | `Cultivo.cs` |
| Points por cosecha | `nivel × 2` | `Cultivo.cs` |
| XP para subir | `50 + (nivel - 1)^2.2 × 15` | `Jugador.cs` |
| Celdas por tramo | Nivel 10 → 3; 20 → 6; 30 → 9; 40 → 12 | `CeldasHuerta.cs` |
| Skins de cultivo | Umbrales cada 5 niveles, de 5 a 45 | `CultivosSkin.cs` |
| Cooldown de golpe | 350ms | `Cultivo.cs` |
| Cooldown de cosecha | `10 / (1 + (nivel - 1) × 0.1)` segundos | `Cultivo.cs` |
| Conversión | `10 Points = 1 PP Coin` | `appsettings.json` + `ConversorServicio` |
| Saldo inicial | 0 Points / 0 PP Coins | `JugadorRepositorio` |

### 6.3 Reglas de UX económica

1. **Mostrar siempre la ganancia:** `+XP` y `+P` (Points) en el popup; saldo actualizado en el HUD. Hoy el popup usa `+PP`, pendiente de unificar.
2. **Mostrar siempre el costo:** precio unitario, cantidad, total y saldo resultante.
3. **Confirmar lo irreversible:** conversión, compra, venta y publicación.
4. **No esconder la tasa:** `10 Points = 1 PP Coin` visible antes de convertir.
5. **No romper el ritmo:** la cosecha no debe abrir un modal; el feedback es local.
6. **No castigar sin explicar:** si falta saldo, decir cuánto falta.
7. **No usar decimales innecesarios:** PP Coins se muestran con `0.##`; internamente, migrar a enteros en centavos para evitar errores de `double`.
8. **No mezclar monedas:** Points y PP Coins tienen símbolos, colores y nombres distintos.

### 6.4 Curva de progresión: intención

- **Primeros minutos:** cosechar es rápido; el primer nivel llega pronto; la primera conversión es alcanzable.
- **Primera hora:** el jugador descubre la tienda, compra una herramienta mejor y siente el cambio de eficiencia.
- **Sesión media:** aparece el mercado; publicar y comprar conecta con otros jugadores.
- **Largo plazo:** más celdas, skins, rarezas y herramientas; el guardado se vuelve un ritual.

**Regla de diseño:** el progreso debe ser **visible en pantalla**, no solo en números. Cada hito debe cambiar algo del mundo: una celda más, una planta distinta, una herramienta equipada, un color nuevo.

### 6.5 Riesgos de diseño

- **Inflación de PP Coins:** si la conversión es demasiado generosa, la tienda pierde sentido. La tasa y los precios deben calibrarse juntos.
- **Frustración por cooldown:** 10s de cosecha puede ser mucho al inicio; el jugador debe entender que la herramienta mejora el ritmo.
- **Ambigüedad de monedas:** resolver el naming PP/PC antes de agregar más pantallas.
- **Mercado vacío:** sin seed de publicaciones, la pestaña “Intercambiar” se siente muerta. La demo necesita dos jugadores y publicaciones activas.
- **Persistencia:** si “Equipar” o “Guardar” no persisten, la fantasía de “partida” se rompe.

---

## 7. Accesibilidad y ergonomía

### 7.1 Contraste y color

- El texto principal `--tf-text` sobre `--tf-panel` tiene buen contraste; el texto muted es solo para metadatos.
- El oro `#ffd84d` sobre madera `#382419` es fuerte; no usar oro sobre pasto claro.
- El verde de acción `#258a46` con texto blanco debe verificarse en cada botón; si falla, oscurecer el fondo o usar texto con sombra.
- No usar color como única señal: la rareza tiene etiqueta; la vida tiene barra + umbral; el error tiene texto.
- Evitar rojo/verde como único diferenciador para daltonismo; agregar icono o texto.

### 7.2 Tipografía y legibilidad

- Press Start 2P a 8px es aceptable para labels cortos, no para párrafos.
- Mínimo recomendado: 10px para texto corrido; 12px para textos largos en móvil.
- Evitar mayúsculas sostenidas en descripciones largas.
- Mantener `line-height` de 1.3–1.5 en Silkscreen.
- Permitir zoom del navegador; no bloquear `user-scalable`.

### 7.3 Teclado y foco

- Todos los botones, inputs, selects y enlaces deben ser alcanzables con `Tab`.
- Foco visible: borde oro de 2px + `outline: none` solo si el borde reemplaza al foco.
- `Esc` cierra modales.
- `Enter` envía formularios; `Espacio` activa botones.
- El foco debe quedar atrapado dentro del modal abierto y volver al disparador al cerrar.
- En Home, las celdas son botones: deben tener `aria-label` (“Cosechar cultivo”) y estado.

### 7.4 Touch

- Área táctil mínima de 44×44px.
- El botón de cerrar de 28×28px es demasiado chico en móvil; aumentar el área con padding invisible.
- Evitar `hover` como única fuente de información.
- El cursor custom no aplica en touch; usar el icono de herramienta dentro de la celda o en el frame equipado.
- En móvil, el frame equipado fijo puede tapar contenido; reposicionarlo.

### 7.5 Movimiento

- El pasto y el título se mueven siempre; es parte de la identidad.
- Agregar una opción “Reducir movimiento” en Configuración que detenga:
  - scroll del pasto,
  - flotación del título,
  - animaciones de crecimiento/desaparición (dejar un fade simple).
- No depender solo de `prefers-reduced-motion`; el sistema operativo puede no reflejar la preferencia del jugador.
- Evitar parpadeos fuertes; el flash de golpe no debe superar 3 destellos por segundo.

### 7.6 Errores y recuperación

- Mensajes en español, sin códigos técnicos.
- Siempre ofrecer una salida: “Reintentar”, “Volver”, “Cerrar”.
- No perder el estado del formulario ante un error.
- Los errores de red no deben cerrar la sesión.
- Los errores de validación deben indicar el campo.

### 7.7 Internacionalización

- Idioma base: español rioplatense (`es-AR`).
- No dejar textos en inglés en la UI: `Not Found`, `Loading`, `Sorry...`, `Save complete`.
- Mantener acentos y signos de apertura (`¿`, `¡`).
- Si se agrega otro idioma, los textos deben vivir en recursos, no incrustados en `.razor`.
- Los números y precios deben respetar la cultura local o ser explícitamente invariantes; no mezclar `,` y `.` decimales.

---

## 8. Implementación y gobernanza

### 8.1 Mapa de CSS

| Archivo | Responsabilidad | Regla |
|---|---|---|
| `wwwroot/css/pixel.css` | Tokens, base, pasto animado, título, auth, botones globales, barra de vida, popup | **Fuente de verdad de tokens** |
| `wwwroot/css/modales.css` | Modal, cabecera, cierre, saldo, mensajes, tarjetas, inputs, botones de modal, rarezas | Solo clases usadas por modales |
| `wwwroot/css/app.css` | Reset del framework, errores de Blazor, loading | No agregar diseño de juego |
| `Pages/*.razor.css` | Layout específico de página | No duplicar tokens ni estilos globales |
| `Components/**/*.razor.css` | Estilos scopeados del componente | Solo lo que no se comparte |
| `wwwroot/index.html` | Carga de fuentes y CSS | Orden: app → scoped → pixel → modales |

**Regla de scope:** si una clase se usa en más de un componente o se renderiza desde un componente hijo, va a `pixel.css` o `modales.css`, no a un `.razor.css`.

### 8.2 Cómo agregar un token

1. Confirmar que el color no existe ya en la paleta.
2. Nombrarlo por función (`--tf-...`), no por apariencia.
3. Agregarlo en `pixel.css` dentro del bloque correspondiente.
4. Usarlo en al menos un componente.
5. Documentarlo en la tabla de [§2.2](#22-paleta).
6. Verificar contraste.

### 8.3 Cómo agregar una pantalla

1. Elegir el shell: juego, auth, modal o terminal.
2. Reutilizar tokens y componentes existentes.
3. Definir los estados: inicial, cargando, vacío, error, éxito, sin permiso.
4. Definir el copy en español rioplatense.
5. Agregar `PageTitle`.
6. Verificar responsive en 390, 768 y 1440.
7. Verificar teclado, foco y `Esc`.
8. Capturar screenshot y comparar con el norte visual.
9. No agregar CSS inline ni `border-radius`.
10. Documentar la ruta en [§5.1](#51-mapa-de-rutas).

### 8.4 Cómo agregar un componente

- Nombre en PascalCase (`TiendaModal.razor`).
- Estilos en `.razor.css` solo si son exclusivos.
- Parámetros explícitos; eventos con `EventCallback`.
- Estados internos: `cargando`, `error`, `mensaje`, `ocupado`.
- `aria-label` en botones icónicos.
- No acceder a servicios desde el markup; usar `@code`.
- No hardcodear IDs de jugador (`2`, `20`): usar `JugadorState`.

### 8.5 Pipeline de assets

1. Dibujar en resolución nativa (16/32/64px).
2. Exportar PNG con transparencia, sin antialias.
3. Nombrar en kebab-case.
4. Guardar en `/images/{categoría}/`.
5. Probar a ×1, ×2 y ×3.
6. Verificar que `image-rendering: pixelated` esté aplicado.
7. Actualizar la tabla de assets de [§2.6](#26-imágenes-sprites-e-iconos).

### 8.6 Convenciones de código visual

- Variables CSS en `kebab-case` con prefijo `--tf-`.
- Clases en español para el dominio (`barra-lateral`, `huerta-celda`, `boton-primario`) y en inglés solo si ya existen.
- Evitar `!important`; si hace falta, es señal de especificidad mal resuelta.
- Evitar selectores de etiqueta globales (`h1`, `p`) dentro de componentes; usar clases.
- Evitar `style=""` en `.razor`; mover a clase.
- Evitar duplicar bloques CSS en dos archivos.
- Comentar solo lo que no es obvio; el código actual tiene buenos comentarios de decisiones.

### 8.7 Checklist de PR visual

- [ ] Usa tokens `--tf-*`, no hex sueltos.
- [ ] Radio 0 en todos los elementos nuevos.
- [ ] Bisel/sombra coherente con el nivel de elevación.
- [ ] Tipografía correcta (Press Start 2P para display; Silkscreen para cuerpo).
- [ ] Mayúsculas por CSS.
- [ ] Estados hover/active/focus/disabled implementados.
- [ ] Estado cargando/vacío/error implementado.
- [ ] Copy en español rioplatense, sin typos.
- [ ] Monedas con símbolos canónicos (`P` / `PC`).
- [ ] Responsive verificado en 390/768/1440.
- [ ] Teclado y `Esc` verificados.
- [ ] Sin `border-radius`, sin blur, sin gradientes fuera de los permitidos.
- [ ] Sin estilos inline nuevos.
- [ ] Sin CSS duplicado.
- [ ] Screenshot comparado con la referencia.

### 8.8 Reglas de oro (resumen ejecutivo)

1. **Si parece una web, está mal.**
2. **Si tiene radio, está mal.**
3. **Si la sombra es difusa, está mal.**
4. **Si el texto largo está en Press Start 2P, está mal.**
5. **Si hay más de tres acentos, está mal.**
6. **Si el botón no se hunde, está mal.**
7. **Si el error no explica qué hacer, está mal.**
8. **Si Points y PP Coins se confunden, está mal.**
9. **Si el estado vacío no invita a actuar, está mal.**
10. **Si el jugador no siente que está en una granja, está mal.**

---

## 9. Estado actual y deuda de diseño

### 9.1 Lo que ya está alineado

- Home, login, registro y modales de tienda/mochila comparten la paleta de madera, pasto y oro.
- Los tokens `--tf-*` están centralizados en `pixel.css`.
- Los botones principales, secundarios, de equipar y de cerrar tienen bisel y estados físicos.
- La huerta tiene cerca, celdas cuadradas, barra de vida y animaciones de cultivo.
- El popup de cosecha comunica XP y Points con códigos de color consistentes.
- Los modales tienen overlay, cabecera, cierre, saldo, mensajes y scrollbar pixelada.
- El HUD mantiene identidad, progreso, monedas y acciones siempre visibles.
- Las capturas responsive de Home muestran que la estructura resiste 390, 768, 1440 y 1920.

### 9.2 Deudas priorizadas

#### P0 — Rompen la inmersión o la claridad

| Deuda | Impacto | Acción |
|---|---|---|
| `/internet` y `/intercambios` usan estética web clara, tarjetas blancas y bordes redondeados | Cortan la fantasía; parecen otro producto | Migrar al shell de juego/terminal; reutilizar tokens y componentes |
| Ambigüedad `PP` (Points) vs `PP Coins` (moneda) | El jugador no sabe qué gasta | Canonizar `P` y `PC`; actualizar HUD, tienda, mercado y conversión |
| `Internet.razor` tiene bloques de código con la etiqueta `razor` al inicio y al final | Se renderizan como texto y rompen la pantalla | Eliminar los fences y el código muerto |
| “?? Equipar” en la mochila | Se ve como un bug | Reemplazar por icono pixel o texto limpio |
| Estilos inline en Home/Internet/Intercambios | Dificultan el mantenimiento y la consistencia | Mover a clases y tokens |
| IDs de jugador hardcodeados (`2`, `20`) | Riesgo de mostrar datos de otro jugador | Usar `JugadorState` y sesión |

#### P1 — Consistencia y accesibilidad

| Deuda | Impacto | Acción |
|---|---|---|
| Skins de cultivo 4–9 sin asset | Posibles 404 al subir de nivel | Crear assets o fallback a la última skin existente |
| Emojis como iconos principales | Dependen del sistema operativo y rompen el píxel | Reemplazar por sprites; mantener emoji solo como fallback |
| `Tienda.razor.css` e `Inventario.razor.css` son legado con radios y estilos claros | Código muerto que confunde | Eliminar o reescribir; los componentes usan `modales.css` |
| Foco de teclado no siempre visible | Accesibilidad | Agregar `:focus-visible` dorado y probar tabulación |
| Modales sin foco atrapado ni `Esc` | Accesibilidad | Implementar focus trap y cierre con teclado |
| Texto de 8px en descripciones | Legibilidad | Subir cuerpo a 10px en modales; mantener 8px solo para metadatos |
| Botón de cerrar de 28px | Touch | Aumentar área táctil a 44px con padding |
| Pasto/título ignoran reduced motion | Accesibilidad | Agregar opción “Reducir movimiento” en Configuración |
| `NotFound` en inglés | Inconsistencia de idioma | Traducir y aplicar shell |

#### P2 — Pulido y contenido

| Deuda | Impacto | Acción |
|---|---|---|
| Solo 3 recursos seed, todos comunes, sin potenciadores/dispositivos | Tienda pobre | Ampliar seed por tipo y rareza; completar descripciones |
| Sin hotbar de 5 slots | La referencia de diseño no está explotada | Implementar hotbar y teclas 1–5 |
| Sin flores decorativas en la cerca | Menos detalle de mundo | Reintroducir acentos pixel en el marco |
| Sin sonido | Menos tacto | Chiptune suave + foley de golpe/cosecha/moneda, con mute |
| Sin onboarding | Curva inicial | Tutorial de 3 pasos: golpear, cosechar, convertir |
| Sin ajustes reales | Configuración vacía | Panel de ajustes: volumen, movimiento, idioma, cuenta |
| Nivel 40+ sin celdas nuevas | Progresión se corta | Definir tramo 50 o cap explícito |
| `CeldasHuerta` puede fallar al superar el último umbral | Excepción en runtime | Agregar fallback al máximo |

### 9.3 Decisiones pendientes

1. **Naming de monedas:** `P`/`PC` vs `Points`/`PP Coins` vs `PP`/`PC`. Recomendación: `P` y `PC` en HUD; `Points` y `PP Coins` en texto.
2. **Shell de auth:** madera (actual) vs navy/terminal (referencia). Recomendación: madera para no partir la experiencia; navy solo si se convierte en “modo terminal”.
3. **Emoji vs sprite:** reemplazo total o convivencia. Recomendación: sprites para iconos de juego; emoji solo en copy.
4. **Internet:** ¿página completa o modal de computadora? Recomendación: ventana terminal dentro del shell de juego.
5. **Rango de compra:** 1–3 (backlog) vs 1–99 (UI actual). Recomendación: definir por tipo de ítem y validar en servidor.
6. **Persistencia de equipado:** en memoria vs servidor. Recomendación: servidor, porque la fantasía de “partida guardada” lo exige.

### 9.4 Roadmap visual sugerido

**Semana 1 — Coherencia P0**
- Migrar `/intercambios` al shell de juego.
- Migrar `/internet` a terminal.
- Canonizar monedas y corregir labels.
- Limpiar fences, `??`, inline styles.

**Semana 2 — Accesibilidad y componentes**
- Focus visible, `Esc`, focus trap.
- Subir tamaños de texto críticos.
- Reemplazar emojis principales por sprites.
- Ajustar touch targets.

**Semana 3 — Contenido y pulido**
- Assets de cultivo 4–9.
- Seed de tienda por tipo/rareza.
- Hotbar y flores de la cerca.
- Sonido y opción de reducir movimiento.

### 9.5 Definition of Done visual

Una pantalla está lista cuando:

- [ ] Usa el shell correcto y se siente parte del mismo juego.
- [ ] No tiene radios, blur indebido ni gradientes fuera de la paleta.
- [ ] Usa tokens y tipografías canónicas.
- [ ] Tiene estados inicial/cargando/vacío/error/éxito.
- [ ] El copy está en español rioplatense y sin typos.
- [ ] Las monedas se distinguen sin ambigüedad.
- [ ] Funciona con teclado y en 390/768/1440.
- [ ] No tiene estilos inline ni CSS duplicado.
- [ ] La captura se parece al norte visual, no a un dashboard.

---

## 10. Anexos

### A. Referencia rápida de tokens

```css
:root {
  /* Madera */
  --tf-panel: #382419;
  --tf-panel-deep: #140b07;
  --tf-panel-bevel-hi: #5c3c28;
  --tf-panel-bevel-lo: #22140c;
  --tf-header: #23160e;
  --tf-header-lo: #3a2214;
  --tf-content: #2c1b12;
  --tf-card: #1e130c;
  --tf-card-hi: #3a2517;
  --tf-card-lo: #0c0603;

  /* Cerca */
  --tf-fence: #3e2213;
  --tf-fence-hi: #5e341c;
  --tf-fence-outline: #140904;

  /* Tierra */
  --tf-soil: #4a240f;
  --tf-soil-hi: #6d3819;
  --tf-soil-lo: #1e0d04;
  --tf-grid-bg: #2a1307;
  --tf-grid-border: #190a03;

  /* Pasto */
  --tf-grass: #8bc34a;
  --tf-grass-light: #7cb342;
  --tf-grass-dark: #8cd634;
  --tf-grass-dark-light: #78be28;

  /* Inputs */
  --tf-input-bg: #0f0906;
  --tf-input-border: #4a2c1b;
  --tf-input-focus: #ffd84d;

  /* Acentos */
  --tf-gold: #ffd84d;
  --tf-gold-hi: #ffcc00;
  --tf-gold-lo: #8a5a00;
  --tf-green: #258a46;
  --tf-green-hover: #2da353;
  --tf-green-bevel-hi: #5fe088;
  --tf-green-bevel-lo: #135227;
  --tf-green-border: #0d381b;
  --tf-red: #b92b27;
  --tf-red-hover: #d43531;
  --tf-red-bevel-hi: #e65c58;
  --tf-red-bevel-lo: #6e1513;
  --tf-red-border: #520e0c;

  /* Pizarra */
  --tf-slate: #2b3945;
  --tf-slate-hover: #3b4b59;
  --tf-slate-border: #111827;
  --tf-slate-bevel-hi: #475569;
  --tf-slate-bevel-lo: #0f172a;

  /* Texto */
  --tf-text: #f7eed7;
  --tf-text-dim: #cfbda8;
  --tf-text-mute: #bfa588;
  --tf-stat: #f6c876;
  --tf-price: #ffe66d;
  --tf-ok-text: #7aff9b;
  --tf-ok: #55ff99;

  /* Rarezas */
  --tf-rarity-comun: #a3a3a3;
  --tf-rarity-raro: #38bdf8;
  --tf-rarity-epico: #c084fc;

  /* Error */
  --tf-error-bg: #4a1512;
  --tf-error-border: #2a0a08;
  --tf-error-text: #ff9d97;

  /* Tipografía */
  --tf-font-pixel: 'Press Start 2P', monospace;
  --tf-font-retro: 'Silkscreen', monospace;
}
```

### B. Mapa de archivos

| Archivo | Qué contiene |
|---|---|
| `PPFramWA.Client/Pages/Home.razor` | HUD, sidebar, huerta, modales, guardado |
| `Pages/Home.razor.css` | Layout del shell de juego |
| `Components/Huerta/HuertaCeldasComponent.razor` | Cerca, grid y celdas |
| `Components/Huerta/CultivoComponent.razor` | Cultivo, barra de vida, popup, animaciones |
| `Components/TiendaModal.razor` | Tienda y compra |
| `Components/InventarioModal.razor` | Mochila, venta y equipado |
| `Pages/Internet.razor` | Computadora: conversión y acceso al mercado |
| `Pages/Intercambios.razor` | Mercado: publicar, listar, comprar |
| `Pages/Login.razor` / `Register.razor` | Autenticación |
| `wwwroot/css/pixel.css` | Tokens y estilos globales |
| `wwwroot/css/modales.css` | Estilos de modal, tarjetas y formularios |
| `wwwroot/index.html` | Fuentes y orden de CSS |
| `DesingFiles/stitch_ppfarm_design_system/*` | Referencias de diseño |
| `Capturas/*` | Evidencia visual responsive |

### C. Copy deck mínimo

| Situación | Copy |
|---|---|
| Título de juego | `PP FARM` |
| Login | `INICIAR SESIÓN` |
| Registro | `CREAR CUENTA` |
| Home: tienda | `TIENDA` |
| Home: mochila | `MOCHILA` |
| Home: computadora | `COMPUTADORA` |
| Home: mercado | `INTERCAMBIOS` |
| Home: ajustes | `CONFIGURACIÓN` |
| Home: guardar | `GUARDAR PARTIDA` |
| Guardado OK | `Partida sincronizada con éxito.` |
| Guardado error | `No se pudo guardar la partida. Reintentá.` |
| Tienda: título | `TIENDA DE LA HUERTA` |
| Tienda: subtítulo | `Conseguí recursos para tu huerta.` |
| Tienda: comprar | `COMPRAR` |
| Tienda: sin saldo | `No tenés suficientes PP Coins.` |
| Mochila: título | `MI INVENTARIO` |
| Mochila: subtítulo | `Equipá o vendé tus recursos.` |
| Mochila: vacío | `Tu inventario está vacío.` |
| Mochila: vender | `VENDER` |
| Mochila: equipar | `EQUIPAR` |
| Mochila: equipado | `EQUIPADO` |
| Computadora: convertir | `CONVERTIR` |
| Computadora: tasa | `10 Points = 1 PP Coin` |
| Computadora: confirmar | `CONFIRMAR CONVERSIÓN` |
| Computadora: error | `Necesitás al menos 10 Points para convertir.` |
| Mercado: título | `INTERCAMBIOS` |
| Mercado: publicar | `PUBLICAR INTERCAMBIO` |
| Mercado: vacío | `No hay intercambios disponibles.` |
| Mercado: comprar | `COMPRAR` |
| Mercado: confirmar | `CONFIRMAR COMPRA` |
| Error de red | `No pudimos conectar con la granja. Reintentá.` |
| 404 | `Esta parcela no existe.` |

### D. Checklist de revisión rápida

```text
[ ] ¿Se siente como PP Farm y no como una web genérica?
[ ] ¿Usa madera, pasto, tierra y oro con la función correcta?
[ ] ¿Tiene radio 0, bisel y sombra dura?
[ ] ¿La tipografía es Press Start 2P / Silkscreen?
[ ] ¿Hay un solo botón primario por decisión?
[ ] ¿Los estados hover/active/focus/disabled se sienten físicos?
[ ] ¿Los estados cargando/vacío/error/éxito están cubiertos?
[ ] ¿Las monedas dicen P y PC sin ambigüedad?
[ ] ¿El copy está en español rioplatense y sin typos?
[ ] ¿Funciona en 390, 768 y 1440?
[ ] ¿Funciona con teclado y Esc?
[ ] ¿Evita inline styles, CSS duplicado y emojis como única iconografía?
```

### E. Changelog

| Versión | Fecha | Cambio |
|---|---|---|
| 1.0 | Octubre 2026 | Creación del documento maestro a partir del código actual, referencias Stitch y capturas. |

---

> **Cierre:** PP Farm no necesita “más estilo”; necesita **coherencia**. La huerta ya tiene una identidad fuerte. La tarea de diseño es llevarla a cada pantalla, especialmente a la computadora y al mercado, sin perder el tacto de píxel, la calidez de la madera y la claridad de la economía. Cuando el jugador golpea un cultivo, convierte Points y compra una herramienta, todo debe sentirse parte del mismo mundo: **una granja de 16 bits que cabe en una terminal**.
