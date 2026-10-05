---
name: Retro Farmstead HUD
colors:
  surface: '#101320'
  surface-dim: '#101320'
  surface-bright: '#363848'
  surface-container-lowest: '#0a0d1b'
  surface-container-low: '#181b29'
  surface-container: '#1c1f2d'
  surface-container-high: '#262938'
  surface-container-highest: '#313443'
  on-surface: '#e0e1f5'
  on-surface-variant: '#c1cab5'
  inverse-surface: '#e0e1f5'
  inverse-on-surface: '#2d303f'
  outline: '#8b9481'
  outline-variant: '#41493a'
  surface-tint: '#90da60'
  primary: '#90da60'
  on-primary: '#153800'
  primary-container: '#6cb33f'
  on-primary-container: '#194100'
  inverse-primary: '#2f6c00'
  secondary: '#febf2b'
  on-secondary: '#412d00'
  secondary-container: '#dfa400'
  on-secondary-container: '#563d00'
  tertiary: '#bdc5e9'
  on-tertiary: '#272f4c'
  tertiary-container: '#98a0c3'
  on-tertiary-container: '#2f3754'
  error: '#ffb4ab'
  on-error: '#690005'
  error-container: '#93000a'
  on-error-container: '#ffdad6'
  primary-fixed: '#aaf779'
  primary-fixed-dim: '#90da60'
  on-primary-fixed: '#092100'
  on-primary-fixed-variant: '#225100'
  secondary-fixed: '#ffdea4'
  secondary-fixed-dim: '#fbbc28'
  on-secondary-fixed: '#261900'
  on-secondary-fixed-variant: '#5d4200'
  tertiary-fixed: '#dce1ff'
  tertiary-fixed-dim: '#bdc5e9'
  on-tertiary-fixed: '#121a36'
  on-tertiary-fixed-variant: '#3e4663'
  background: '#101320'
  on-background: '#e0e1f5'
  surface-variant: '#313443'
typography:
  headline-xl:
    fontFamily: Rubik
    fontSize: 32px
    fontWeight: '800'
    lineHeight: 40px
    letterSpacing: 0.04em
  headline-xl-mobile:
    fontFamily: Rubik
    fontSize: 24px
    fontWeight: '800'
    lineHeight: 32px
    letterSpacing: 0.03em
  headline-lg:
    fontFamily: Rubik
    fontSize: 22px
    fontWeight: '700'
    lineHeight: 28px
    letterSpacing: 0.03em
  headline-md:
    fontFamily: Rubik
    fontSize: 18px
    fontWeight: '700'
    lineHeight: 24px
    letterSpacing: 0.02em
  body-lg:
    fontFamily: Rubik
    fontSize: 16px
    fontWeight: '500'
    lineHeight: 24px
    letterSpacing: 0.01em
  body-md:
    fontFamily: Rubik
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
    letterSpacing: 0em
  label-lg:
    fontFamily: Rubik
    fontSize: 14px
    fontWeight: '700'
    lineHeight: 18px
    letterSpacing: 0.05em
  label-md:
    fontFamily: Rubik
    fontSize: 12px
    fontWeight: '700'
    lineHeight: 16px
    letterSpacing: 0.06em
  label-sm:
    fontFamily: Rubik
    fontSize: 10px
    fontWeight: '700'
    lineHeight: 14px
    letterSpacing: 0.08em
spacing:
  gutter: 1rem
  gutter-mobile: 0.5rem
  margin: 1.5rem
  margin-mobile: 1rem
  space-xs: 0.25rem
  space-sm: 0.5rem
  space-md: 1rem
  space-lg: 1.5rem
  space-xl: 2rem
---

## Brand & Style

The visual narrative bridges cozy retro farming simulation mechanics with a high-contrast, tactile pixel-art game interface. Designed for cross-platform simulation players engaging in crop cultivation, marketplace commerce, inventory management, and farm progression, the interface provides immediate game-state legibility with tactile micro-interactions.

The movement sits directly within **Tactile Arcade Retro**:
- **Hard Bevels & Multi-Line Frames**: Surfaces use multi-tone step borders to mimic 16-bit physical cartridges and CRT-era dialog frames rather than soft Gaussian drops.
- **Deep Recessed Wells**: Interactive slots, inventory inventories, and data inputs feel stamped into the dark slate substrate.
- **Punchy Verdant & Golden Cues**: The visual baseline pairs dark oceanic navy slate with luminous pasture greens and punchy harvest golds, establishing clear functional priority across inventory and trade flows.

## Colors

The palette reproduces the contrast between outdoor field gameplay and dark slate modal dialogues:

- **Pasture Grass & Action Greens**: `#6cb33f` serves as the primary ground token, paired with `#5fa035` for checkered plot tiles. `#2e7d32` powers primary interaction surfaces, brightening to `#388e3c` on hover, grounded by `#1b4d20` for physical push-down pixel shadows.
- **Harvest Gold & Economy**: `#f1b31c` anchors currency counters, modal titles, rarity accents, and focus indicators, reinforced by `#e09f12` for border bevels and `#ffcc00` for crisp highlight pips.
- **Slate & Navy Modal Shells**: Structural surfaces rely on `#181b29` (base backdrops/canvases), `#1f2438` (dialog body wells), and `#252b42` (raised container cards).
- **Hard Pixel Framing**: `#3f4765` serves as the midtone border, `#535d82` acts as top-left specular edge highlights, `#33384f` drives secondary button faces, and `#0d1017` creates structural shadow lines and deep recessed cutouts (`#121520`).

## Typography

The type system is powered by Rubik's sturdy, rounded-geometric construction, optimized for micro-scale legibility and retro gaming menus:

- **Case Strategy**: Modal titles, header banners, slot badges, and key call-to-actions are strictly styled with `text-transform: uppercase` to emulate arcade status bars.
- **Weight Pairing**: Body text stays lean and legible with medium (`500`) and regular (`400`) weights on recessed `#121520` or `#1f2438` tiles. All metadata labels, coin numbers, and button actions utilize bold (`700`) and extrabold (`800`) weights.
- **Letter Spacing**: Extended letter-spacing (`0.04em` to `0.08em`) across labels prevents dense pixel clustering at 10px–14px sizes.

## Layout & Spacing

Layouts adhere to an 8-bit grid discipline built on strict 4px and 8px step increments:

- **Modal Windows & Viewports**: Centered modal cards (e.g., `TiendaModal`, `InventarioModal`) maintain standard desktop widths of `560px` to `720px` with a fixed `1.5rem` canvas margin. On mobile devices (<640px), modal sheets expand to full screen minus an edge margin of `1rem`.
- **Game Farm Board**: The central tile grid utilizes a checkered repeating unit layout (32px to 64px square tiles) nested inside scroll-safe bounds without subpixel stretching.
- **Slot Matrix Spacing**: Item grids in storage and store views employ `space-sm` (`0.5rem` / 8px) gaps to ensure individual selection borders never merge or clip.

## Elevation & Depth

Depth is produced exclusively through physical beveled lines and hard directional shadows rather than blurred atmospheric layers:

- **Modal Layer (Elevation Level 3)**: Multi-step perimeter stacking. Uses a `4px solid #0d1017` outer structural edge, a `2px solid #535d82` top/left highlight, and a `2px solid #181b29` bottom/right shadow frame. Modal backdrops use `rgba(13, 16, 23, 0.85)` without blur.
- **Raised Interactive Buttons (Elevation Level 2)**: Action triggers feature an extruded bottom shelf: `border-bottom: 4px solid #1b4d20` on primary green buttons and `border-bottom: 4px solid #1b1e2c` on slate secondary buttons.
- **Engraved / Recessed Slots (Elevation Level -1)**: Inventory cells and text inputs sit behind an inner rim using `box-shadow: inset 0 2px 0 0 #0d1017, inset 0 -1px 0 0 #252b42` to give the appearance of an embedded chassis slot.

## Shapes

The geometric signature is absolute zero-radius (`0px`), adhering faithfully to the pixel-art genre:

- **Sharp Edge Geometry**: All buttons, dialog containers, item slots, popups, and input bars feature 90-degree orthogonal edges.
- **Corner Notches**: Optional retro corner clipping is achieved strictly via stepped pixel cutouts (e.g., 2px corner drop) rather than CSS `border-radius`.
- **Iconography Framing**: All sprite containers, coin indicators, and avatar windows sit in square or crisp rectangular aspect ratios.

## Components

### Buttons
- **Primary Game Action**: Background `#2e7d32`, text `#ffffff`, border `2px solid #535d82`, with bottom lip `border-bottom: 4px solid #1b4d20`. Hover triggers background `#388e3c`. Active click moves the transform down `translateY(2px)` and compresses the bottom border to `2px`.
- **Secondary Slate Action**: Background `#33384f`, text `#ffffff`, border `2px solid #3f4765`, with bottom lip `border-bottom: 4px solid #1b1e2c`. Hover changes background to `#3f4663`.

### Input Fields (`Login.razor`, `Register.razor`)
- **Base State**: Background `#121520`, text `#ffffff`, border `2px solid #33394f`, padding `0.5rem 0.75rem`.
- **Focus State**: Border updates instantly to `2px solid #e09f12`, with zero outline blur and an active golden cursor.

### Modals (`TiendaModal.razor`, `InventarioModal.razor`)
- **Shell**: Base background `#1f2438` framed with an outer outline `2px solid #0d1017`, an inner border `3px solid #3f4765`, and top specular highlights `#535d82`.
- **Header Header Strip**: Background `#181b29`, featuring uppercase titles styled in `#f1b31c` with a bottom border `2px solid #3f4765`.
- **Close Button**: Hard red/slate square (`28px x 28px`), border `2px solid #0d1017`, hover state `#d32f2f`.

### Inventory & Store Slots
- **Slot Container**: Square unit (`56px x 56px`), background `#121520`, border `2px solid #33394f`.
- **Selected Slot**: Border turns `2px solid #ffcc00` with an inset highlight of `inset 0 0 0 2px #f1b31c`.
- **Quantity Badge**: Floating bottom-right badge, zero-radius, background `#0d1017`, text `#ffffff`, typography `label-sm`.

### Checkboxes & Radios
- **Frame**: Square `18px x 18px`, background `#121520`, border `2px solid #33394f`.
- **Checked Indicator**: Solid `#6cb33f` inner pixel block (`10px x 10px`) centered with sharp margins.