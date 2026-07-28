# SocialMediaAssistant Typography Guide

## Font Pairing

| Role | Font | Source |
|---|---|---|
| **Headings / Display** | Plus Jakarta Sans | Google Fonts |
| **Body / UI** | Inter | Google Fonts |
| **Code / Mono** | JetBrains Mono | Google Fonts |

---

## Google Fonts Import

```html
<!-- In your <head> -->
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800&family=Inter:wght@400;500;600;700&family=JetBrains+Mono:wght@400;500&display=swap" rel="stylesheet">
```

```css
/* Or via @import in CSS */
@import url('https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800&family=Inter:wght@400;500;600;700&family=JetBrains+Mono:wght@400;500&display=swap');
```

---

## Type Scale

| Token | Size | px | Usage |
|---|---|---|---|
| `--text-xs`   | 0.75rem  | 12px | Captions, legal text |
| `--text-sm`   | 0.875rem | 14px | Labels, helper text |
| `--text-base` | 1rem     | 16px | Body copy (default) |
| `--text-lg`   | 1.125rem | 18px | Lead paragraphs |
| `--text-xl`   | 1.25rem  | 20px | Large body, card subtitles |
| `--text-2xl`  | 1.5rem   | 24px | H6 / section subheadings |
| `--text-3xl`  | 1.875rem | 30px | H5 |
| `--text-4xl`  | 2.25rem  | 36px | H4 |
| `--text-5xl`  | 3rem     | 48px | H3 |
| `--text-6xl`  | 3.75rem  | 60px | H2 |
| `--text-7xl`  | 4.5rem   | 72px | H1 / Hero |

---

## Heading Styles (Plus Jakarta Sans)

| Level | Size | Weight | Line Height | Letter Spacing | Usage |
|---|---|---|---|---|---|
| H1 | 72px (4.5rem) | 800 ExtraBold | 1.1 | −0.05em | Hero / page title |
| H2 | 60px (3.75rem) | 800 ExtraBold | 1.15 | −0.04em | Section title |
| H3 | 48px (3rem)   | 700 Bold     | 1.2  | −0.03em | Sub-section title |
| H4 | 36px (2.25rem) | 700 Bold    | 1.25 | −0.02em | Card headings |
| H5 | 30px (1.875rem) | 600 SemiBold | 1.3 | −0.01em | List headings |
| H6 | 24px (1.5rem) | 600 SemiBold  | 1.35 | 0em    | Small headings |

---

## Body Styles (Inter)

| Style | Size | Weight | Line Height | Letter Spacing | Usage |
|---|---|---|---|---|---|
| Body Large  | 18px (1.125rem) | 400 Regular | 1.625 | 0em     | Lead paragraph |
| Body Base   | 16px (1rem)     | 400 Regular | 1.5   | 0em     | Default body |
| Body Small  | 14px (0.875rem) | 400 Regular | 1.5   | 0.01em  | Secondary text |
| Label       | 14px (0.875rem) | 600 SemiBold | 1.25 | 0.05em  | Form labels, tags |
| Caption     | 12px (0.75rem)  | 400 Regular | 1.5   | 0.025em | Captions, footnotes |
| Overline    | 11px (0.6875rem)| 700 Bold    | 1    | 0.15em  | Section labels (UPPERCASE) |

---

## Font Weights Reference

| Weight | Value | Usage |
|---|---|---|
| Regular   | 400 | Body copy, descriptions |
| Medium    | 500 | UI labels, navigation |
| SemiBold  | 600 | Sub-headings, buttons, labels |
| Bold      | 700 | Headings H3–H6, strong emphasis |
| ExtraBold | 800 | H1, H2, hero text, brand wordmark |

---

## CSS Usage Examples

```css
/* H1 — Hero headline */
h1 {
  font-family: var(--font-heading);
  font-size: var(--text-7xl);        /* 72px */
  font-weight: var(--weight-extrabold); /* 800 */
  line-height: var(--leading-none);  /* 1 */
  letter-spacing: var(--tracking-tighter); /* −0.05em */
  color: var(--color-text-light);
}

/* H2 — Section title */
h2 {
  font-family: var(--font-heading);
  font-size: var(--text-6xl);        /* 60px */
  font-weight: var(--weight-extrabold);
  line-height: var(--leading-tight); /* 1.25 */
  letter-spacing: var(--tracking-tight); /* −0.025em */
}

/* Body copy */
p {
  font-family: var(--font-body);
  font-size: var(--text-base);       /* 16px */
  font-weight: var(--weight-regular);
  line-height: var(--leading-relaxed); /* 1.625 */
  letter-spacing: var(--tracking-normal); /* 0 */
}

/* Overline label (e.g. "NEW FEATURE") */
.overline {
  font-family: var(--font-body);
  font-size: 11px;
  font-weight: var(--weight-bold);
  line-height: 1;
  letter-spacing: var(--tracking-widest); /* 0.1em */
  text-transform: uppercase;
}

/* Gradient headline text */
.gradient-text {
  background: var(--color-primary-gradient);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}
```

---

## Design Principles

1. **Hierarchy first** — Use size and weight contrast to guide the eye, not colour alone.
2. **Limit weights** — Use only 400, 600, and 800 in any single layout for clarity.
3. **Plus Jakarta Sans for punch** — Reserve for headlines, CTAs, and the brand wordmark.
4. **Inter for readability** — All body copy, UI elements, forms, and dashboards.
5. **Tight headlines** — Apply negative letter spacing (−0.03em to −0.05em) on large headings.
6. **Generous body line-height** — Use 1.5–1.625 for comfortable reading in dashboards.
7. **ALL CAPS sparingly** — Use only for overline labels (e.g. "NEW FEATURE", "PHASE 1") at small sizes with wide tracking.
