uniform sampler2D sceneTex;

vec3 sunColour = vec3(1.f, 1.f, 1.f);
vec3 skyColour = vec3(0.2f, 0.2f, 0.2f);
vec2 sunPosition = vec2(0.5f, 0.5f);
const float sunSize = 0.2f;
 
void main() {
    if (texture2D(sceneTex, gl_TexCoord[0].xy).a > 0.5f) discard;

    if (distance(gl_TexCoord[0].xy, sunPosition) < sunSize) {
        gl_FragColor = vec4(sunColour, 1.f);
    } else {
        gl_FragColor = vec4(skyColour, 1.f);
    }
}