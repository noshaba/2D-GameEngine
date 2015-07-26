uniform sampler2D sceneTex;
uniform vec2 moonPosition;
uniform float moonRadius;
uniform vec2 resolution;
uniform vec3 moonColour;
uniform vec3 skyColour;
 
void main() {
    if (texture2D(sceneTex, gl_TexCoord[0].xy).a > 0.5f) discard;

    if (distance(gl_TexCoord[0].xy * resolution, moonPosition * resolution) < moonRadius) {
        gl_FragColor = vec4(moonColour, 1.f);
    } else {
        gl_FragColor = vec4(skyColour, 1.f);
    }
}