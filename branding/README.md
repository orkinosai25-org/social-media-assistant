# SocialMediaAssistant Branding Assets

Welcome to the SocialMediaAssistant brand asset library. This folder contains all official branding materials for the **SocialMediaAssistant** product.

---

## 📁 Asset Index

```
branding/
├── README.md                          ← You are here
├── brand-guidelines.md                ← Full brand guidelines (start here)
│
├── logo/
│   ├── logo.svg                       ← Default horizontal lockup (200×60)
│   ├── logo-dark.svg                  ← Dark background variant
│   ├── logo-light.svg                 ← Light background variant
│   └── logo-icon.svg                  ← Icon only (60×60)
│
├── banners/
│   ├── hero-banner.svg                ← Landing page / social hero (1200×630)
│   ├── twitter-banner.svg             ← Twitter/X profile banner (1500×500)
│   └── og-image.svg                   ← Open Graph link preview (1200×630)
│
├── colours/
│   ├── palette.svg                    ← Visual colour reference sheet
│   ├── tokens.css                     ← CSS custom properties (design tokens)
│   └── tokens.json                    ← JSON tokens for Figma / Style Dictionary
│
├── typography/
│   └── type-scale.md                  ← Typography guide & font pairing
│
└── social-media/
    ├── instagram-post-template.svg    ← Instagram square post (1080×1080)
    └── instagram-story-template.svg   ← Instagram Story (1080×1920)
```

---

## 🎨 Brand at a Glance

| Element | Value |
|---|---|
| **Product** | SocialMediaAssistant |
| **Tagline** | *"Your AI. Always Selling."* |
| **Primary colour** | `#833AB4` Purple → `#E1306C` Pink gradient |
| **Secondary** | `#1877F2` Facebook Blue |
| **Accent / CTA** | `#25D366` WhatsApp Green |
| **Dark BG** | `#0D0D1A` |
| **Light BG** | `#F8F9FF` |
| **Heading font** | Plus Jakarta Sans (700, 800) |
| **Body font** | Inter (400, 500, 600) |

---

## 🖼️ How to Use Each File

### `brand-guidelines.md`
The complete brand rulebook. Read this before creating any new materials. Covers colours (hex/RGB/HSL), gradients, typography, voice & tone, spacing, and do's & don'ts.

### `logo/logo.svg`
The primary logo. Use on white or neutral grey backgrounds. Never recolour.

### `logo/logo-dark.svg`
Use on dark backgrounds — dashboard header, dark hero sections, dark emails.

### `logo/logo-light.svg`
Use on white/light grey backgrounds — light-mode landing page header, printed materials.

### `logo/logo-icon.svg`
Use as an app icon, browser favicon, social media profile picture, or any context where the full wordmark doesn't fit. Minimum display size: 24×24px.

### `banners/hero-banner.svg`
1200×630px. Use as:
- Landing page hero section background
- Facebook / LinkedIn / Twitter post header
- YouTube channel art (crop to fit)

### `banners/twitter-banner.svg`
1500×500px. Upload directly to Twitter/X profile as the cover image.

### `banners/og-image.svg`
1200×630px. Use as the `og:image` meta tag image for all website pages. Convert to PNG for compatibility:
```html
<meta property="og:image" content="https://socialmediaassistant.ai/og-image.png">
```

### `colours/palette.svg`
A visual reference sheet for designers. Print or share to quickly communicate the colour palette. Not for use in production.

### `colours/tokens.css`
Import into your CSS codebase:
```html
<link rel="stylesheet" href="/branding/colours/tokens.css">
```
Or copy into your main stylesheet. All values are exposed as CSS custom properties (`--color-primary`, `--font-heading`, etc.).

### `colours/tokens.json`
Use with:
- **Figma Tokens plugin** (Token Sets → Import JSON)
- **Style Dictionary** (`style-dictionary build`)
- **Theo** by Salesforce
- Any custom design token pipeline

### `typography/type-scale.md`
Full typography guide including font pairing rationale, type scale table, Google Fonts import code, and CSS usage examples.

### `social-media/instagram-post-template.svg`
1080×1080px Instagram feed post template. Open in Figma or Inkscape, replace the placeholder text boxes with real content, export as PNG.

### `social-media/instagram-story-template.svg`
1080×1920px Instagram Story template. Includes:
- Story progress bar (top)
- Headline + feature bullets placeholder (middle)
- Animated-style ring decoration
- Swipe-up CTA bar (bottom)

Export as PNG at 1× for direct Story upload.

---

## 🛠️ Design Tool Notes

### Figma
1. Import `colours/tokens.json` via the **Figma Tokens** plugin
2. Open any SVG file via `File → Import` or paste SVG code into a frame
3. Set up a Local Style for each gradient using the values in `brand-guidelines.md`
4. Use Auto Layout with spacing tokens from `tokens.json`

### Inkscape
- All SVG files are valid and will open correctly in Inkscape 1.x+
- Gradient definitions are in the `<defs>` block of each SVG

### Adobe Illustrator
- Open SVGs directly — gradients and paths will import correctly
- Use the Swatch panel to add hex values from the palette

---

## ✅ Checklist: Before Publishing Any Asset

- [ ] Logo is the correct variant for the background (dark/light)
- [ ] Brand gradient used for primary CTA / hero element
- [ ] Font is Plus Jakarta Sans for headings, Inter for body
- [ ] Dark background is `#0D0D1A`, not pure black `#000000`
- [ ] Chat bubble motif is present as a decorative element
- [ ] Text contrast meets WCAG AA (4.5:1 minimum)
- [ ] SocialMediaAssistant logo appears in the asset

---

## 🔗 Links

- Full brand guidelines: [`brand-guidelines.md`](./brand-guidelines.md)
- Typography guide: [`typography/type-scale.md`](./typography/type-scale.md)
- CSS tokens: [`colours/tokens.css`](./colours/tokens.css)
- JSON tokens: [`colours/tokens.json`](./colours/tokens.json)
- Project docs: [`../docs/`](../docs/)
