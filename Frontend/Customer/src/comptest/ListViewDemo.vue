<template>
<div class="width-container card pad-1 margin-bottom-1">
    <h2 class="margin-bottom-1">[demo] Colors &ndash; page 0</h2>
    <ListView v-if="colors" :items="colors" 
        :view="ColorView" 
        :bind="(item) => ({ color: item })" 
        class="flex-gallery flex-spacing-1" />
</div>    
</template>

<script>
import ListView from "@/comp/ListView.vue"
import ColorView from "@/comp/ColorView.vue"

function data() {
    return {
        colors: [ ]
    }
}

function mounted() {
    this.$axios.get({ url: "/catalog/colors/list?page=0" })
    .then(({ status, data: response }) => {
        if (response.items instanceof Array) {
            this.colors = response.items
        }
    })
    .catch(error => {
        console.error(error)
    })
}

export default {
    data,
    mounted,
    components: {
        ListView,
        ColorView
    },
    computed: {
        ColorView: () => ColorView
    }
}
</script>