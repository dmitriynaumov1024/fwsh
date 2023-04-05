<template>
<div class="width-container card pad-1 margin-bottom-1">
    <h2 class="margin-bottom-1">
        Order #{{order.id}}
    </h2>
    <table class="kvtable stripes">
        <template v-for="(value, key) in order">
            <tr v-if="typeof value != 'object'">
                <td>{{ key }}</td>
                <td>{{ value }}</td>
            </tr>
        </template>
    </table>
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