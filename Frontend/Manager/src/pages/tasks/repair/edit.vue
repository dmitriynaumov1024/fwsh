<template>
<Bread>
    <Crumb to="/">fwsh</Crumb>
    <Crumb to="/tasks">{{locale.task.plural}}</Crumb>
    <Crumb :to="`/tasks/repair/${props.tab??'list'}?page=0`">{{locale.repairTask.plural}}</Crumb>
    <Crumb last>#{{props.id}}</Crumb>
</Bread>
<div class="width-container card pad-1">
    <RepairTaskEdit v-if="data.task" 
        :task="data.task"
        :badFields="data.badFields"
        :errorMessage="data.errorMessage"
        :successMessage="data.successMessage" 
        @click-reset="resetTask"
        @click-submit="submitTask" />
    <template v-else>
        <h2 class="mar-b-1">{{locale.noData.title}}</h2>
        <p>{{locale.noData.description}}</p>
    </template>
</div>

</template>

<script>
    
</script>
