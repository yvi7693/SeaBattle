import os
import random
import struct
import zlib


OUT_DIR = os.path.join("Assets", "Sprites", "UI")


def clamp(value, minimum=0, maximum=255):
    return max(minimum, min(maximum, int(value)))


def lerp(a, b, t):
    return a + (b - a) * t


def mix(c1, c2, t):
    return tuple(clamp(lerp(c1[i], c2[i], t)) for i in range(4))


def rounded_rect_alpha(x, y, w, h, radius):
    px = x + 0.5
    py = y + 0.5
    cx = min(max(px, radius), w - radius)
    cy = min(max(py, radius), h - radius)
    dx = px - cx
    dy = py - cy
    distance = (dx * dx + dy * dy) ** 0.5
    return clamp((radius + 0.8 - distance) * 255)


def stroke_alpha(x, y, w, h, outer_radius, inner_radius, inset):
    outer = rounded_rect_alpha(x, y, w, h, outer_radius)
    inner = rounded_rect_alpha(x - inset, y - inset, w - inset * 2, h - inset * 2, inner_radius)
    return clamp(outer - inner)


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


def downsample(source, source_w, source_h, scale):
    target_w = source_w // scale
    target_h = source_h // scale
    output = bytearray(target_w * target_h * 4)
    for y in range(target_h):
        for x in range(target_w):
            totals = [0, 0, 0, 0]
            for sy in range(scale):
                for sx in range(scale):
                    idx = ((y * scale + sy) * source_w + (x * scale + sx)) * 4
                    for i in range(4):
                        totals[i] += source[idx + i]
            out_idx = (y * target_w + x) * 4
            count = scale * scale
            for i in range(4):
                output[out_idx + i] = clamp(totals[i] / count)
    return output, target_w, target_h


def render_button(filename, top, bottom, glow, press_offset=0, brightness=1.0):
    scale = 3
    w, h = 512 * scale, 256 * scale
    pixels = bytearray(w * h * 4)
    radius = 50 * scale
    inset = 11 * scale

    for y in range(h):
        for x in range(w):
            a = rounded_rect_alpha(x, y, w, h, radius)
            if a <= 0:
                continue

            t = y / max(1, h - 1)
            base = mix(top, bottom, t)

            center_x = (x / w - 0.5) * 2
            center_y = (y / h - 0.5) * 2
            vignette = max(0.0, 1.0 - (center_x * center_x * 0.35 + center_y * center_y * 0.2))
            wave = 0.08 * (1 + __import__("math").sin((x / w) * 15 + (y / h) * 5))
            light = (0.82 + 0.24 * vignette + wave) * brightness

            r, g, b, _ = base
            color = (clamp(r * light), clamp(g * light), clamp(b * light), a)

            highlight_y = int((46 + press_offset) * scale)
            highlight_width = 24 * scale
            if abs(y - highlight_y) < highlight_width and x > 48 * scale and x < 464 * scale:
                strength = (1 - abs(y - highlight_y) / highlight_width) * 0.28
                color = mix(color, (*glow[:3], a), strength)

            border = stroke_alpha(x, y, w, h, radius, radius - inset, inset)
            if border > 0:
                border_color = (124, 202, 230, border)
                color = mix(color, border_color, min(0.85, border / 255))

            inner_line = stroke_alpha(x - 18 * scale, y - 18 * scale, w - 36 * scale, h - 36 * scale, radius - 18 * scale, radius - 25 * scale, 7 * scale)
            if inner_line > 0:
                color = mix(color, (210, 244, 255, a), min(0.35, inner_line / 255))

            # Small corner rivets/bolts, kept subtle so TMP labels stay readable.
            for bx in (58 * scale, 454 * scale):
                for by in (60 * scale, 196 * scale):
                    d = ((x - bx) ** 2 + (y - by) ** 2) ** 0.5
                    if d < 8 * scale:
                        bolt = max(0, 1 - d / (8 * scale))
                        color = mix(color, (178, 223, 232, a), bolt * 0.5)

            idx = (y * w + x) * 4
            pixels[idx : idx + 4] = bytes(color)

    small, sw, sh = downsample(pixels, w, h, scale)
    write_png(os.path.join(OUT_DIR, filename), sw, sh, small)


def make_meta(filename, guid, sprite_name):
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
  spritePixelsToUnits: 100
  spriteBorder: {{x: 42, y: 42, z: 42, w: 42}}
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
    textureCompression: 0
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
    spriteID: {guid[:16]}0800000000000000
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
    render_button(
        "naval_button_normal.png",
        top=(27, 70, 92, 255),
        bottom=(8, 28, 42, 255),
        glow=(78, 191, 224, 255),
    )
    render_button(
        "naval_button_hover.png",
        top=(42, 93, 116, 255),
        bottom=(13, 39, 57, 255),
        glow=(128, 229, 255, 255),
        brightness=1.13,
    )
    render_button(
        "naval_button_pressed.png",
        top=(10, 36, 52, 255),
        bottom=(5, 21, 34, 255),
        glow=(41, 144, 183, 255),
        press_offset=10,
        brightness=0.86,
    )

    random.seed(24601)
    for filename in (
        "naval_button_normal.png",
        "naval_button_hover.png",
        "naval_button_pressed.png",
    ):
        guid = "".join(random.choice("0123456789abcdef") for _ in range(32))
        sprite_name = filename[:-4]
        with open(os.path.join(OUT_DIR, filename + ".meta"), "w", encoding="utf-8") as f:
            f.write(make_meta(filename, guid, sprite_name))


if __name__ == "__main__":
    main()
