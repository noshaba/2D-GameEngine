uniform vec2 lightPosition = vec2(0.5f, 0.5f);
uniform sampler2D texture;

const int NUM_SAMPLES = 30;
const float exposure = 0.8f;
const float density = 0.5f;
const float weight = 0.1f;

void main() {	
    vec2 texCrd = gl_TexCoord[0].st;
    vec2 deltaTexCrd = vec2(texCrd - lightPosition.xy);
    deltaTexCrd *= 1.f / float(NUM_SAMPLES) * density;

    for (int i = 0; i < NUM_SAMPLES; i++) {
        texCrd -= deltaTexCrd;
        vec3 texel = texture2D(texture, texCrd).rgb;
        gl_FragColor.rgb += texel * weight;
    }

    gl_FragColor.rgb *= exposure;
    gl_FragColor.a = 1.f;
}