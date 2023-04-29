<template>
<template v-if="state.data">
    <slot v-if="!noDefault" name="default" :data="state.data">
        <div :class="props.classError">
            <h2>Data was fetched, but there is no markup in default slot.</h2>
            <p>{{state.data}}</p>
        </div>
    </slot>
</template>
<template v-if="state.loading">
    <slot name="loading">
        <div :class="props.classError">
            <h2 class="margin-bottom-05">{{locale.loading.title}}</h2>
            <p>{{locale.loading.description}}</p>
        </div>
    </slot>
</template>
<template v-if="state.unauthorized">
    <slot name="unauthorized">
        <div :class="props.classError">
            <h2 class="margin-bottom-05">{{locale.unauthorized.title}}</h2>
            <p class="margin-bottom-2">{{locale.unauthorized.description}}</p>
            <div class="flex-stripe flex-pad-1">
                <button class="button button-primary" 
                    @click="()=> router.push('/login')">{{locale.action.login}}</button>
                <button class="button button-secondary" 
                    @click="()=> router.push('/signup')">{{locale.action.signup}}</button>
                <div class="flex-grow"></div>
            </div>
        </div>
    </slot>
</template>
<template v-if="state.badRequest">
    <slot name="unauthorized">
        <div :class="props.classError">
            <h2 class="margin-bottom-05">{{locale.badRequest.title}}</h2>
            <p>{{locale.badRequest.description}}</p>
        </div>
    </slot>
</template>
<template v-if="state.notFound">
    <slot name="notFound">
        <div :class="props.classError">
            <h2 class="margin-bottom-05">{{locale.notFound.title}}</h2>
            <p>{{locale.notFound.description}}</p>
        </div>
    </slot>
</template> 
<template v-if="state.noData">
    <slot name="noData">
        <div :class="props.classError">
            <h2 class="margin-bottom-05">{{locale.noData.title}}</h2>
            <p>{{locale.noData.description}}</p>
        </div>
    </slot>
</template>
<template v-if="state.error">
    <slot name="error">
        <div :class="props.classError">
            <h2 class="margin-bottom-05">{{locale.common.somethingWrong}}</h2>
            <p>{{locale.common.seeConsole}}</p>
        </div>
    </slot>
</template>
</template>

<script setup>
import { useRouter } from "vue-router"
import { reactive, watch, inject } from "vue"

const locale = inject("locale")
const axios = inject("axios")
const router = useRouter()

const props = defineProps({
    url: String,
    params: Object,
    cacheTTL: Number,
    noDefault: Boolean,
    classError: {
        type: undefined,
        default: "card pad-1 margin-bottom-1"
    }
})

const state = reactive({
    data: undefined,
    loading: false,
    unauthorized: false,
    badRequest: false,
    notFound: false,
    noData: false,
    error: false,
    reset() {
        this.data = undefined
        this.loading = true
        this.unauthorized = false
        this.badRequest = false
        this.notFound = false
        this.noData = false
        this.error = false
    }
})

const emit = defineEmits([
    "load",
    "unauthorized",
    "badrequest",
    "notfound",
    "nodata",
    "error"
])

watch ([ ()=> props.url, ()=> props.params ], dispatchRequest, { immediate: true })

function dispatchRequest() {
    state.reset()
    setTimeout(request, (200 + Math.random()*400) | 0)
}

function request() {
    axios.get({
        url: props.url,
        params: props.params,
        cacheTTL: props.cacheTTL
    })
    .then(({ status, data }) => {
        state.loading = false
        if (status == 200) {
            if (data != null && data != undefined) {
                state.data = data
                emit("load", data)
            }
            else {
                state.noData = true
                emit("nodata")
            }
        }
        else if (status == 400) {
            state.badRequest = true
            emit("badrequest")
        }
        else if (status == 401 || status == 403) {
            state.unauthorized = true
            emit("unauthorized")
        }
        else if (status == 404) {
            state.notFound = true
            emit("notfound")
        }
        else {
            state.error = true
            emit("error")
        }
    })
    .catch((error) => {
        state.loading = false
        state.error = true
        emit("error")
    })
}

</script>
