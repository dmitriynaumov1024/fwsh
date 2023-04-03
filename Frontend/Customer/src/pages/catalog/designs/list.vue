<template>
<div class="width-container card pad-1 margin-bottom-1">
    <PaginationView v-if="items?.length"
        :page="page" :previous="previous" :next="next"
        :items="items" :view="$options.components.MiniDesignView"
        :bind="item => ({ design: item })"
        @click-previous="goToPrevious"
        @click-next="goToNext"
        @click-item="goToItem" />
</div>
</template>

<script>
import PaginationView from "@/comp/PaginationView.vue"
import MiniDesignView from "@/comp/MiniDesignView.vue"

const props = {
    page: Number
}

function data() {
    return {
        previous: null,
        next: null,
        items: [ ]
    }
}

function goToPrevious() {
    if (this.previous != null)
        this.$router.push(`/catalog/designs/list?page=${this.previous}`)
}

function goToNext() {
    if (this.next != null)
        this.$router.push(`/catalog/designs/list?page=${this.next}`)
}

function goToItem(item) {
    console.log("Should go to " + item.id)
}

function mounted() {
    console.log(this.$options.components)
    this.$axios.get({
        url: "/catalog/designs/list",
        params: { page: this.page }
    })
    .then(({ status, data: response }) => {
        this.items = response.items
        this.previous = response.previous
        this.next = response.next
    })
    .catch(error => {
        console.error(error)
    })
}

export default {
    props,
    data,
    mounted,
    methods: {
        goToPrevious,
        goToNext,
        goToItem
    },
    components: {
        PaginationView,
        MiniDesignView
    }
}
</script>