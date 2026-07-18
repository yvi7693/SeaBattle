from __future__ import annotations

from dataclasses import dataclass
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
OUT_DIR = ROOT / "Assets" / "Models" / "Ships"
CELL = 1.0
WIDTH = 0.62
HULL_HEIGHT = 0.16
DECK_HEIGHT = 0.05


@dataclass
class Mesh:
    vertices: list[tuple[float, float, float]]
    faces: list[tuple[str, tuple[int, ...]]]

    def __init__(self) -> None:
        self.vertices = []
        self.faces = []

    def add_vertex(self, point: tuple[float, float, float]) -> int:
        self.vertices.append(point)
        return len(self.vertices)

    def add_face(self, material: str, indices: tuple[int, ...]) -> None:
        self.faces.append((material, indices))


def add_box(mesh: Mesh, center: tuple[float, float, float], size: tuple[float, float, float], material: str) -> None:
    cx, cy, cz = center
    sx, sy, sz = (value * 0.5 for value in size)
    points = [
        (cx - sx, cy - sy, cz - sz),
        (cx + sx, cy - sy, cz - sz),
        (cx + sx, cy + sy, cz - sz),
        (cx - sx, cy + sy, cz - sz),
        (cx - sx, cy - sy, cz + sz),
        (cx + sx, cy - sy, cz + sz),
        (cx + sx, cy + sy, cz + sz),
        (cx - sx, cy + sy, cz + sz),
    ]
    ids = [mesh.add_vertex(point) for point in points]
    for face in (
        (0, 1, 2, 3),
        (4, 7, 6, 5),
        (0, 4, 5, 1),
        (1, 5, 6, 2),
        (2, 6, 7, 3),
        (3, 7, 4, 0),
    ):
        mesh.add_face(material, tuple(ids[index] for index in face))


def add_hull(mesh: Mesh, decks: int) -> None:
    length = decks * CELL
    half = length * 0.5
    w = WIDTH * 0.5
    keel = -HULL_HEIGHT
    top = 0.0
    bow = half + 0.22
    stern = -half - 0.16
    inset = 0.13

    points = [
        (stern, -w * 0.68, keel),
        (half - inset, -w, keel),
        (bow, 0.0, keel),
        (half - inset, w, keel),
        (stern, w * 0.68, keel),
        (-half, -w, top),
        (half - inset, -w, top),
        (bow, 0.0, top),
        (half - inset, w, top),
        (-half, w, top),
    ]
    ids = [mesh.add_vertex(point) for point in points]
    for face in (
        (0, 1, 2, 3, 4),
        (5, 9, 8, 7, 6),
        (0, 5, 6, 1),
        (1, 6, 7, 2),
        (2, 7, 8, 3),
        (3, 8, 9, 4),
        (4, 9, 5, 0),
    ):
        mesh.add_face("HullGraphite", tuple(ids[index] for index in face))


def add_deck(mesh: Mesh, decks: int) -> None:
    length = decks * CELL - 0.14
    add_box(mesh, (0.0, 0.0, DECK_HEIGHT * 0.5), (length, WIDTH * 0.70, DECK_HEIGHT), "DeckMist")

    for index in range(decks):
        x = -decks * CELL * 0.5 + CELL * 0.5 + index * CELL
        add_box(mesh, (x, 0.0, DECK_HEIGHT + 0.012), (0.66, WIDTH * 0.46, 0.018), "PanelIce")

    bridge_x = -decks * CELL * 0.5 + min(0.55, decks * 0.18 + 0.18)
    bridge_w = 0.38 if decks == 1 else 0.48
    bridge_l = 0.32 if decks == 1 else 0.44
    add_box(mesh, (bridge_x, 0.0, 0.16), (bridge_l, bridge_w, 0.18), "BridgeWhite")

    if decks >= 2:
        mast_x = bridge_x + 0.18
        add_box(mesh, (mast_x, 0.0, 0.33), (0.055, 0.055, 0.22), "AccentBlue")

    if decks >= 3:
        add_box(mesh, (0.28, -WIDTH * 0.23, 0.11), (0.18, 0.06, 0.08), "AccentBlue")
        add_box(mesh, (0.28, WIDTH * 0.23, 0.11), (0.18, 0.06, 0.08), "AccentBlue")

    if decks == 4:
        add_box(mesh, (1.05, 0.0, 0.12), (0.38, 0.18, 0.10), "BridgeWhite")
        add_box(mesh, (-1.15, 0.0, 0.105), (0.24, 0.14, 0.07), "AccentBlue")


def write_obj(path: Path, decks: int) -> None:
    mesh = Mesh()
    add_hull(mesh, decks)
    add_deck(mesh, decks)

    lines = [
        f"# Minimal flat modern {decks}-deck ship for SeaBattle",
        "mtllib minimal_fleet.mtl",
        f"o ship_{decks}_deck_minimal",
    ]
    for x, y, z in mesh.vertices:
        lines.append(f"v {x:.4f} {z:.4f} {y:.4f}")

    current_material = None
    for material, face in mesh.faces:
        if material != current_material:
            lines.append(f"usemtl {material}")
            current_material = material
        lines.append("f " + " ".join(str(index) for index in face))

    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_mtl(path: Path) -> None:
    path.write_text(
        "\n".join(
            [
                "# Shared flat materials for the SeaBattle minimal fleet",
                "newmtl HullGraphite",
                "Kd 0.18 0.22 0.26",
                "Ks 0.08 0.10 0.12",
                "Ns 24",
                "",
                "newmtl DeckMist",
                "Kd 0.78 0.83 0.86",
                "Ks 0.10 0.12 0.13",
                "Ns 18",
                "",
                "newmtl PanelIce",
                "Kd 0.92 0.96 0.98",
                "Ks 0.08 0.10 0.10",
                "Ns 12",
                "",
                "newmtl BridgeWhite",
                "Kd 0.96 0.96 0.93",
                "Ks 0.12 0.12 0.10",
                "Ns 16",
                "",
                "newmtl AccentBlue",
                "Kd 0.18 0.54 0.74",
                "Ks 0.20 0.30 0.34",
                "Ns 32",
            ]
        )
        + "\n",
        encoding="utf-8",
    )


def write_readme(path: Path) -> None:
    path.write_text(
        "\n".join(
            [
                "# Minimal Flat Ship Models",
                "",
                "Generated OBJ models for the SeaBattle fleet:",
                "",
                "- `ship_1_deck_minimal.obj`",
                "- `ship_2_deck_minimal.obj`",
                "- `ship_3_deck_minimal.obj`",
                "- `ship_4_deck_minimal.obj`",
                "",
                "The models are low-poly, flat-shaded, centered on the X axis, and sized so each deck occupies one Unity unit.",
                "They share `minimal_fleet.mtl` for graphite hulls, light decks, and restrained blue accents.",
                "",
                "Regenerate them with:",
                "",
                "```bash",
                "python3 Tools/generate_minimal_ship_models.py",
                "```",
            ]
        )
        + "\n",
        encoding="utf-8",
    )


def main() -> None:
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    write_mtl(OUT_DIR / "minimal_fleet.mtl")
    for decks in range(1, 5):
        write_obj(OUT_DIR / f"ship_{decks}_deck_minimal.obj", decks)
    write_readme(OUT_DIR / "README.md")


if __name__ == "__main__":
    main()
