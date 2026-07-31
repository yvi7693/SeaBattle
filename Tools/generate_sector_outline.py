import os
import struct
import zlib


OUT_DIR = os.path.join("Assets", "Sprites", "UI")

# Геометрия совпадает с sector_square_rounded.png: 128x128, full-bleed, радиус 13.4
# (замерен подгонкой по альфе клетки). Внешний край рамки повторяет силуэт клетки
# один в один, поэтому INSET = 0: любой отступ пришлось бы компенсировать радиусом.
SIZE = 128
INSET = 0.0      # отступ внешнего края от границы текстуры
RADIUS = 13.4    # радиус скругления внешнего контура — как у клетки
WIDTH = 8.0      # толщина линии

GUID = "b7d41c2f9a6e84d1cb35f0a7e2d94816"


def clamp(value, minimum=0, maximum=255):
    return max(minimum, min(maximum, int(value)))


def clamp01(value):
    return max(0.0, min(1.0, value))


def rounded_box_sdf(px, py, cx, cy, half_w, half_h, radius):
    """Расстояние до края скруглённого прямоугольника: <0 внутри, >0 снаружи."""
    qx = abs(px - cx) - (half_w - radius)
    qy = abs(py - cy) - (half_h - radius)
    ax = max(qx, 0.0)
    ay = max(qy, 0.0)
    return (ax * ax + ay * ay) ** 0.5 + min(max(qx, qy), 0.0) - radius


def render_outline(width, height, inset, radius, thickness):
    pixels = bytearray(width * height * 4)

    cx = width / 2.0
    cy = height / 2.0

    outer_half_w = width / 2.0 - inset
    outer_half_h = height / 2.0 - inset
    outer_radius = radius

    inner_half_w = outer_half_w - thickness
    inner_half_h = outer_half_h - thickness
    inner_radius = max(outer_radius - thickness, 0.5)

    for y in range(height):
        for x in range(width):
            px = x + 0.5
            py = y + 0.5

            outer = rounded_box_sdf(px, py, cx, cy, outer_half_w, outer_half_h, outer_radius)
            inner = rounded_box_sdf(px, py, cx, cy, inner_half_w, inner_half_h, inner_radius)

            # Полоса между внешним и внутренним контуром, со сглаживанием в 1px.
            alpha = clamp01(0.5 - outer) * clamp01(0.5 + inner)

            idx = (y * width + x) * 4
            pixels[idx : idx + 4] = bytes((255, 255, 255, clamp(alpha * 255)))

    return pixels


def write_png(path, width, height, pixels):
    raw = bytearray()
    for y in range(height):
        raw.append(0)
        row_start = y * width * 4
        raw.extend(pixels[row_start : row_start + width * 4])

    def chunk(name, data):
        body = name + data
        return struct.pack(">I", len(data)) + body + struct.pack(">I", zlib.crc32(body) & 0xFFFFFFFF)

    png = b"\x89PNG\r\n\x1a\n"
    png += chunk(b"IHDR", struct.pack(">IIBBBBB", width, height, 8, 6, 0, 0, 0))
    png += chunk(b"IDAT", zlib.compress(bytes(raw), 9))
    png += chunk(b"IEND", b"")

    with open(path, "wb") as f:
        f.write(png)


def make_meta(guid):
    return f"""fileFormatVersion: 2
guid: {guid}
TextureImporter:
  internalIDToNameTable: []
  externalObjects: {{}}
  serializedVersion: 13
  mipmaps:
    mipMapMode: 0
    enableMipMap: 0
    sRGBTexture: 1
    linearTexture: 0
    fadeOut: 0
    borderMipMap: 0
    mipMapsPreserveCoverage: 0
    alphaTestReferenceValue: 0.5
    mipMapFadeDistanceStart: 1
    mipMapFadeDistanceEnd: 3
  bumpmap:
    convertToNormalMap: 0
    externalNormalMap: 0
    heightScale: 0.25
    normalMapFilter: 0
    flipGreenChannel: 0
  isReadable: 0
  streamingMipmaps: 0
  streamingMipmapsPriority: 0
  vTOnly: 0
  ignoreMipmapLimit: 0
  grayScaleToAlpha: 0
  generateCubemap: 6
  cubemapConvolution: 0
  seamlessCubemap: 0
  textureFormat: 1
  maxTextureSize: 2048
  textureSettings:
    serializedVersion: 2
    filterMode: 1
    aniso: 1
    mipBias: 0
    wrapU: 1
    wrapV: 1
    wrapW: 1
  nPOTScale: 0
  lightmap: 0
  compressionQuality: 50
  spriteMode: 1
  spriteExtrude: 1
  spriteMeshType: 1
  alignment: 0
  spritePivot: {{x: 0.5, y: 0.5}}
  spritePixelsToUnits: 128
  spriteBorder: {{x: 0, y: 0, z: 0, w: 0}}
  spriteGenerateFallbackPhysicsShape: 1
  alphaUsage: 1
  alphaIsTransparency: 1
  spriteTessellationDetail: -1
  textureType: 8
  textureShape: 1
  singleChannelComponent: 0
  flipbookRows: 1
  flipbookColumns: 1
  maxTextureSizeSet: 0
  compressionQualitySet: 0
  textureFormatSet: 0
  ignorePngGamma: 0
  applyGammaDecoding: 0
  swizzle: 50462976
  cookieLightType: 0
  platformSettings:
  - serializedVersion: 4
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  spriteSheet:
    serializedVersion: 2
    sprites: []
    outline: []
    customData:
    physicsShape: []
    bones: []
    spriteID: 5e97ba1c8d24f3a60800000000000000
    internalID: 0
    vertices: []
    indices:
    edges: []
    weights: []
    secondaryTextures: []
    spriteCustomMetadata:
      entries: []
    nameFileIdTable: {{}}
  mipmapLimitGroupName:
  pSDRemoveMatte: 0
  userData:
  assetBundleName:
  assetBundleVariant:
"""


def main():
    os.makedirs(OUT_DIR, exist_ok=True)

    pixels = render_outline(SIZE, SIZE, INSET, RADIUS, WIDTH)
    write_png(os.path.join(OUT_DIR, "sector_outline.png"), SIZE, SIZE, pixels)

    with open(os.path.join(OUT_DIR, "sector_outline.png.meta"), "w", encoding="utf-8") as f:
        f.write(make_meta(GUID))


if __name__ == "__main__":
    main()
