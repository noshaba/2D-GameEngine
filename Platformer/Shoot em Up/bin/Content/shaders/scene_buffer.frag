uniform sampler2D sceneTex;

vec3 sunColour = vec3(0.5f, 0.5f, 1f);
vec3 skyColour = vec3(0f, 0f, 0.5f);
vec2 sunPosition = vec2(0.5f, 0.7f);
const float sunSize = 0.2f;
 
void main() {
    if (texture2D(sceneTex, gl_TexCoord[0].xy).a > 0.5f) discard;

    if (distance(gl_TexCoord[0].xy, sunPosition) < sunSize) {
        gl_FragColor = vec4(sunColour, 1.f);
    } else {
        gl_FragColor = vec4(skyColour, 1.f);
    }
}