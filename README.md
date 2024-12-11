This CLI app allows you to generate an output shader file from `#include`

# usage

## for windows
go to system variables > environment variables > PATH > add the bin folder to the path

then open the cmd in the folder of your shader, and use so like 
`shaderinclude sourceFile.frag outFile.frag`

as an example:

  main.frag:
  ```glsl
  #ifdef GL_ES
precision mediump float;
#endif

#include "colorShift.frag"

void mainImage(out vec4 fragColor, in vec2 fragCoord) {
    vec2 uv = fragCoord.xy / iResolution.xy;
    uv.x *= iResolution.x / iResolution.y;
    vec4 col = vec4(vec3(uv, 0.), 1.);

    col = shift(col);

    fragColor = col;
}
  ```

colorShift.frag:
```glsl
vec4 shift(in vec4 color) {
    return color.bgra;
}

```

result:
```glsl
#ifdef GL_ES
precision mediump float;
#endif

vec4 shift(in vec4 color) {
    return color.bgra;
}


void mainImage(out vec4 fragColor, in vec2 fragCoord) {
    vec2 uv = fragCoord.xy / iResolution.xy;
    uv.x *= iResolution.x / iResolution.y;
    vec4 col = vec4(vec3(uv, 0.), 1.);

    col = shift(col);

    fragColor = col;
}

```
