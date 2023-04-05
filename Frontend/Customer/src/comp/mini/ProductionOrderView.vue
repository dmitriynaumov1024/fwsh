<template>
<div class="pad-05">
    <p><b>{{ order.design?.displayName }}</b></p>
    <p>Status: {{ order.status }}</p>
    <p>{{ order.pricePerOne }} &#8372; x {{ order.quantity }} = <b>{{ order.priceTotal }} &#8372;</b></p>
</div>
</template>

<script>
const props = {
    order: Object
}

export default {
    props
}
</script>